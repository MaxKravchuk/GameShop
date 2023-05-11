using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<OrderCreateDTO> _validator;

        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<OrderCreateDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task<int> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(orderCreateDTO);
            var newOrder = _mapper.Map<Order>(orderCreateDTO);
            var games = await _unitOfWork.GameRepository.GetAsync();

            _unitOfWork.OrderRepository.Insert(newOrder);

            foreach (var game in orderCreateDTO.ListOfOrderDetails)
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
            return newOrder.Id;
        }
    }
}
