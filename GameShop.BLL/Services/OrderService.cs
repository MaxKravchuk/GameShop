using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Strategies;
using GameShop.BLL.Strategies.Interfaces;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Repository.Interfaces.Utils;

namespace GameShop.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisProvider<CartItemDTO> _redisProvider;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<OrderCreateDTO> _validator;
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;

        public OrderService(
            IUnitOfWork unitOfWork,
            IRedisProvider<CartItemDTO> redisProvider,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<OrderCreateDTO> validator,
            IPaymentStrategyFactory paymentStrategyFactory)
        {
            _unitOfWork = unitOfWork;
            _redisProvider = redisProvider;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
            _paymentStrategyFactory = paymentStrategyFactory;
        }

        public async Task<PaymentResultDTO> ExecutePayment(OrderCreateDTO orderCreateDTO)
        {
            if (!orderCreateDTO.IsPaymentSuccessful)
            {
                throw new BadRequestException("Payment is not successful");
            }

            var newOrder = await CreateOrderAsync(orderCreateDTO);
            var strategy = _paymentStrategyFactory.GetPaymentStrategy(orderCreateDTO.Strategy);
            return strategy.Pay(newOrder);
        }

        private async Task<Order> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(orderCreateDTO);
            var newOrder = _mapper.Map<Order>(orderCreateDTO);
            _unitOfWork.OrderRepository.Insert(newOrder);

            var redisKey = orderCreateDTO.CustomerID == 0 ? "CartItems" : $"CartItems-{orderCreateDTO.CustomerID}";
            var cartItems = await _redisProvider.GetValuesAsync(redisKey);

            if (!cartItems.Any())
            {
                throw new NotFoundException("Cart is empty");
            }

            var games = await _unitOfWork.GameRepository.GetAsync();

            foreach (var game in cartItems)
            {
                var gameToAdd = games.SingleOrDefault(g => g.Key == game.GameKey);

                if (gameToAdd.UnitsInStock < game.Quantity)
                {
                    throw new BadRequestException("Not enough games");
                }

                var orderDetails = new OrderDetails
                {
                    GameId = gameToAdd.Id,
                    OrderId = newOrder.Id,
                    Quantity = game.Quantity,
                    Discount = 0,
                };

                _unitOfWork.OrderDetailsRepository.Insert(orderDetails);
                gameToAdd.UnitsInStock -= game.Quantity;
                _unitOfWork.GameRepository.Update(gameToAdd);
            }

            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Order with id {newOrder.Id} created succesfully");
            return newOrder;
        }
    }
}
