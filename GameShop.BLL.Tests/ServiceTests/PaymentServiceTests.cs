using System;
using System.Threading.Tasks;
using GameShop.BLL.DTO.PaymentDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Enums;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class PaymentServiceTests : IDisposable
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IPaymentStrategyFactory> _mockPaymentStrategyFactory;
        private readonly PaymentService _paymentService;

        private bool _disposed;

        public PaymentServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockPaymentStrategyFactory = new Mock<IPaymentStrategyFactory>();

            _paymentService = new PaymentService(
                _mockUnitOfWork.Object,
                _mockLogger.Object,
                _mockPaymentStrategyFactory.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task ExecutePaymentAsync_WithCorrectPaymentCreateDTO_ShouldReturnPaymentResult()
        {
            // Arrange
            var order = new Order() { Id = 1 };
            var paymentCreateDTO = new PaymentCreateDTO { OrderId = 1, Strategy = "Bank" };
            var paymentStrategy = new Mock<IPaymentStrategy>();
            var paymentResult = new PaymentResultDTO { OrderId = 1, IsPaymentSuccessful = true };

            _mockUnitOfWork
                .Setup(u => u.OrderRepository
                    .GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(order);

            _mockPaymentStrategyFactory
                .Setup(psf => psf
                    .GetPaymentStrategy(It.IsAny<PaymentTypes>()))
                .Returns(paymentStrategy.Object);

            paymentStrategy
                .Setup(s => s
                    .Pay(It.IsAny<Order>()))
                .Returns(paymentResult);

            // Act
            var result = await _paymentService.ExecutePaymentAsync(paymentCreateDTO);

            // Assert
            _mockUnitOfWork.Verify(u => u.OrderRepository.GetByIdAsync(1, string.Empty), Times.Once);
            Assert.IsType<PaymentResultDTO>(result);
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
                _mockLogger.Invocations.Clear();
                _mockPaymentStrategyFactory.Invocations.Clear();
            }

            _disposed = true;
        }
    }
}
