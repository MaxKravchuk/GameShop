using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.OrderDetailDTOs;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Enums;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Repository.Interfaces.Utils;
using Newtonsoft.Json.Linq;

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

        public async Task<int> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(orderCreateDTO);
            var newOrder = _mapper.Map<Order>(orderCreateDTO);
            newOrder.IsPaid = false;
            _unitOfWork.OrderRepository.Insert(newOrder);

            var redisKey = orderCreateDTO.CustomerId == 1 ? "CartItems" : $"CartItems-{orderCreateDTO.CustomerId}";
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

                var orderDetails = new OrderDetail
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
            return newOrder.Id;
        }

        public async Task<IEnumerable<OrderReadListDTO>> GetAllOrdersAsync()
        {
            var thirtyDaysAgo = System.DateTime.UtcNow.AddDays(-30);

            var orders = await _unitOfWork.OrderRepository
                .GetAsync(
                filter: x => x.OrderedAt >= thirtyDaysAgo,
                includeProperties: "Customer");

            var models = _mapper.Map<IEnumerable<OrderReadListDTO>>(orders);

            _loggerManager.LogInfo($"Orders were returned successfully in array size of {models.Count()}");
            return models;
        }

        public async Task<OrderReadDTO> GetOrderById(int orderId)
        {
            var order = (await _unitOfWork.OrderRepository.GetAsync(
                filter: x => x.Id == orderId,
                includeProperties: "Customer")).SingleOrDefault();
            if (order == null)
            {
                throw new NotFoundException($"Order with id {orderId} was not found");
            }

            var orderDetails = (await _unitOfWork.OrderDetailsRepository.GetAsync(
                filter: x => x.OrderId == order.Id,
                includeProperties: "Game")).ToList();
            order.ListOfOrderDetails = orderDetails;

            var model = _mapper.Map<OrderReadDTO>(order);
            _loggerManager.LogInfo(
                $"Order with id {orderId} was returned");
            return model;
        }

        public async Task UpdateOrderDetailsAsync(OrderUpdateDTO orderUpdateDTO)
        {
            var exOrder = (await _unitOfWork.OrderRepository.GetAsync(
                filter: x => x.Id == orderUpdateDTO.Id,
                includeProperties: "ListOfOrderDetails")).SingleOrDefault();
            if (exOrder == null)
            {
                throw new NotFoundException($"Order with id {orderUpdateDTO.Id} was not found");
            }

            var games = (await _unitOfWork.GameRepository.GetAsync())
                .ToDictionary(x => x.Key);

            foreach (var detail in orderUpdateDTO.OrderDetails)
            {
                if (!games.TryGetValue(detail.GameKey, out var game))
                {
                    throw new BadRequestException($"Game with key {detail.GameKey} was not found");
                }

                if (!IsEnoughGamesAvailable(game, exOrder, detail))
                {
                    throw new BadRequestException("Not enough games");
                }

                var existingOrderDetail = exOrder.ListOfOrderDetails
                    .FirstOrDefault(od => od.GameId == game.Id);

                if (existingOrderDetail != null)
                {
                    existingOrderDetail.Quantity = detail.Quantity;
                    existingOrderDetail.Discount = detail.Discount;
                }
            }

            _unitOfWork.OrderRepository.Update(exOrder);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Order with id {orderUpdateDTO.Id} was updated");
        }

        public async Task UpdateOrderStatusAsync(OrderUpdateDTO orderUpdateDTO)
        {
            var exOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderUpdateDTO.Id);
            OrderStatusTypes newOrderStatusTypes;

            newOrderStatusTypes = orderUpdateDTO.Status.ToEnum<OrderStatusTypes>();
            exOrder.Status = newOrderStatusTypes.ToString();
            exOrder.ShippedDate = DateTime.UtcNow;

            _unitOfWork.OrderRepository.Update(exOrder);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Order with id {orderUpdateDTO.Id} was updated");
        }

        private bool IsEnoughGamesAvailable(Game game, Order exOrder, OrderDetailsUpdateDTO detail)
        {
            var totalQuantityNeeded = detail.Quantity;
            var currentQuantityInOrder = exOrder.ListOfOrderDetails
                .Where(od => od.GameId == game.Id)
                .Sum(od => od.Quantity);

            return game.UnitsInStock + currentQuantityInOrder >= totalQuantityNeeded;
        }
    }
}
