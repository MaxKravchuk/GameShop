using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
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

        public OrderService(
            IUnitOfWork unitOfWork,
            IRedisProvider<CartItemDTO> redisProvider,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<OrderCreateDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _redisProvider = redisProvider;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(orderCreateDTO);
            var newOrder = _mapper.Map<Order>(orderCreateDTO);
            _unitOfWork.OrderRepository.Insert(newOrder);

            var games = await _unitOfWork.GameRepository.GetAsync();
            var redisKey = orderCreateDTO.CustomerID == 0 ? "CartItems" : $"CartItems-{orderCreateDTO.CustomerID}";
            var cartItems = await _redisProvider.GetValuesAsync(redisKey);

            foreach (var game in cartItems)
            {
                var gameToAddId = games.Where(g => g.Key == game.GameKey).SingleOrDefault().Id;
                var orderDetails = new OrderDetails
                {
                    GameId = gameToAddId,
                    OrderId = newOrder.Id,
                    Quantity = game.Quantity,
                    Discount = 0,
                };
                _unitOfWork.OrderDetailsRepository.Insert(orderDetails);
            }

            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Order with id {newOrder.Id} created succesfully");
            return newOrder;
        }
    }
}
