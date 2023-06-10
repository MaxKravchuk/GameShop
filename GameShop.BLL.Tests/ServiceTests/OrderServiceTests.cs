using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.OrderDetailDTOs;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Repository.Interfaces.Utils;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
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
        public async Task CreateOrderAsync_WithCorrectOrder_ShouldReturnOrderCreateDTO()
        {
            // Arrange
            var orderCreateDTO = new OrderCreateDTO
            {
                CustomerId = 1,
                OrderedAt = DateTime.UtcNow
            };
            var gameList = new List<Game> { new Game { Id = 1, Key = "test", UnitsInStock = 2 } };
            var order = new Order { Id = 1 };
            var paymentResult = new PaymentResultDTO { OrderId = 1, IsPaymentSuccessful = true };

            MockCreateOrder(orderCreateDTO, order);

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(gameList);

            _mockUnitOfWork
                .Setup(u => u.OrderDetailsRepository
                    .Insert(It.IsAny<OrderDetail>()))
                .Verifiable();

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .Update(It.IsAny<Game>()))
                .Verifiable();

            // Act
            var result = await _orderService.CreateOrderAsync(orderCreateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.OrderRepository.Insert(order), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsOrdersWithinLast30Days()
        {
            // Arrange
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var orders = new List<Order>
            {
                new Order { Id = 1, OrderedAt = DateTime.UtcNow.AddDays(-40) },
                new Order { Id = 2, OrderedAt = DateTime.UtcNow.AddDays(-20) },
                new Order { Id = 3, OrderedAt = DateTime.UtcNow.AddDays(-10) }
            };
            var ordersDto = new List<OrderReadListDTO>
            {
                new OrderReadListDTO { Id = 1 },
                new OrderReadListDTO { Id = 1 },
            };

            _mockUnitOfWork
                .Setup(u => u.OrderRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Order, bool>>>(),
                        It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(orders.Where(x => x.OrderedAt >= thirtyDaysAgo));

            _mockMapper
                .Setup(m => m.Map<IEnumerable<OrderReadListDTO>>(It.IsAny<IEnumerable<Order>>()))
                .Returns(ordersDto);

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            Assert.Equal(ordersDto.Count, result.Count());
            Assert.Equal(ordersDto.Select(x => x.Id), result.Select(x => x.Id));
        }

        [Fact]
        public async Task GetOrderById_WithCorrectId_ShouldReturnOrder()
        {
            // Arrange
            var orderId = 1;
            var orders = new List<Order>
            {
                new Order { Id = 1 }
            };
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                     Id = 1,
                     OrderId = 1
                }
            };
            var orderReadDto = new OrderReadDTO
            {
                Id = 1
            };

            _mockUnitOfWork
                .Setup(u => u.OrderRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Order, bool>>>(),
                        It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(orders);

            _mockUnitOfWork
                .Setup(u => u.OrderDetailsRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<OrderDetail, bool>>>(),
                        It.IsAny<Func<IQueryable<OrderDetail>, IOrderedQueryable<OrderDetail>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(orderDetails);

            _mockMapper
                .Setup(m => m.Map<OrderReadDTO>(It.IsAny<Order>()))
                .Returns(orderReadDto);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            _mockLogger.Verify(l => l.LogInfo($"Order with id {orderId} was returned"), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderDetailsAsync_UpdatesOrderDetails()
        {
            // Arrange
            var orderId = 1;
            var orderUpdateDTO = new OrderUpdateDTO
            {
                Id = orderId,
                OrderDetails = new List<OrderDetailsUpdateDTO>
                {
                    new OrderDetailsUpdateDTO { GameKey = "key1", Quantity = 5, Discount = 10 },
                    new OrderDetailsUpdateDTO { GameKey = "key2", Quantity = 3, Discount = 5 }
                }
            };

            var existingOrder = new Order
            {
                Id = orderId,
                ListOfOrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { GameId = 1, Quantity = 2, Discount = 0 },
                    new OrderDetail { GameId = 2, Quantity = 1, Discount = 0 }
                }
            };

            var games = new List<Game>
            {
                new Game { Id = 1, Key = "key1", UnitsInStock = 10 },
                new Game { Id = 2, Key = "key2", UnitsInStock = 5 }
            };

            _mockUnitOfWork
                .Setup(u => u.OrderRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Order, bool>>>(),
                        It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(new List<Order> { existingOrder });

            _mockUnitOfWork
                .Setup(u => u.GameRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(games);

            // Act
            await _orderService.UpdateOrderDetailsAsync(orderUpdateDTO);

            // Assert
            Assert.Equal(orderUpdateDTO.OrderDetails.Count(), existingOrder.ListOfOrderDetails.Count);
            _mockUnitOfWork.Verify(x => x.OrderRepository.Update(existingOrder), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
            _mockLogger.Verify(x => x.LogInfo($"Order with id {orderId} was updated"), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderDetailsAsync_ThrowNotFoundForOrder()
        {
            // Arrange
            var orderId = 0;
            var orderUpdateDTO = new OrderUpdateDTO
            {
                Id = orderId,
                OrderDetails = new List<OrderDetailsUpdateDTO>
                {
                    new OrderDetailsUpdateDTO { GameKey = "key1", Quantity = 5, Discount = 10 },
                    new OrderDetailsUpdateDTO { GameKey = "key2", Quantity = 3, Discount = 5 }
                }
            };

            var existingOrder = new Order
            {
                Id = 1,
                ListOfOrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { GameId = 1, Quantity = 2, Discount = 0 },
                    new OrderDetail { GameId = 2, Quantity = 1, Discount = 0 }
                }
            };

            _mockUnitOfWork
                .Setup(u => u.OrderRepository
                    .GetAsync(
                        It.IsAny<Expression<Func<Order, bool>>>(),
                        It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(Enumerable.Empty<Order>());

            // Act
            var result = _orderService.UpdateOrderDetailsAsync(orderUpdateDTO);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_UpdatesOrderStatus()
        {
            // Arrange
            var orderId = 1;
            var orderUpdateDTO = new OrderUpdateDTO
            {
                Id = orderId,
                Status = "Shipped"
            };

            var existingOrder = new Order
            {
                Id = orderId,
                Status = "Unapid",
                ShippedDate = null
            };

            _mockUnitOfWork
                .Setup(u => u.OrderRepository
                    .GetByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                .ReturnsAsync(existingOrder);

            // Act
            await _orderService.UpdateOrderStatusAsync(orderUpdateDTO);

            // Assert
            Assert.Equal(orderUpdateDTO.Status, existingOrder.Status);
            Assert.NotNull(existingOrder.ShippedDate);
            Assert.True(existingOrder.ShippedDate.Value <= DateTime.UtcNow);

            _mockUnitOfWork.Verify(x => x.OrderRepository.Update(existingOrder), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
            _mockLogger.Verify(x => x.LogInfo($"Order with id {orderId} was updated"), Times.Once);
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

        private void MockCreateOrder(OrderCreateDTO orderCreateDTO, Order order)
        {
            var cartItems = new List<CartItemDTO> { new CartItemDTO { GameKey = "test" } };

            _mockMapper.Setup(m => m.Map<Order>(orderCreateDTO)).Returns(order);

            _mockUnitOfWork
                .Setup(x => x
                    .OrderRepository.Insert(It.IsAny<Order>()))
                .Verifiable();

            _mockRedisProvider
                .Setup(r => r
                    .GetValuesAsync(It.IsAny<string>()))
                .ReturnsAsync(cartItems);
        }
    }
}
