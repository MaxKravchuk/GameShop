using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Repository.Interfaces.Utils;
using Moq;
using Xunit;

namespace BLL.Test.ServiceTests
{
    public class OrderServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRedisProvider<CartItemDTO>> _mockRedisProvider;
        private readonly OrderService _orderService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IValidator<OrderCreateDTO>> _mockValidator;

        private bool _disposed;

        public OrderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRedisProvider = new Mock<IRedisProvider<CartItemDTO>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<OrderCreateDTO>>();

            _orderService = new OrderService(
                _mockUnitOfWork.Object,
                _mockRedisProvider.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockValidator.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnOrderNumber()
        {
            // Arrange
            var orderCreateDto = new OrderCreateDTO();
            var order = new Order() { Id = 1 };
            var orderDetails = new OrderDetails { GameId = 1, OrderId = 1 };
            var gameList = new List<Game> { new Game { Id = 1, Key = "test" } };
            var cartItems = new List<CartItemDTO> { new CartItemDTO { GameKey = "test" } };

            _mockMapper.Setup(m => m.Map<Order>(orderCreateDto)).Returns(order);

            _mockUnitOfWork
                .Setup(x => x
                    .OrderRepository.Insert(It.IsAny<Order>()))
                .Verifiable();

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            _mockRedisProvider
                .Setup(r => r
                    .GetValuesAsync(It.IsAny<string>()))
                .ReturnsAsync(cartItems);

            _mockUnitOfWork
                .Setup(u => u.OrderDetailsRepository
                    .Insert(It.IsAny<OrderDetails>()));

            // Act
            var result = await _orderService.CreateOrderAsync(orderCreateDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.OrderRepository.Insert(order), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _mockUnitOfWork.Invocations.Clear();
                _mockMapper.Invocations.Clear();
                _mockLogger.Invocations.Clear();
                _mockValidator.Invocations.Clear();
            }

            _disposed = true;
        }
    }
}
