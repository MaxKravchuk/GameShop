using System.IO;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.PaymentDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests.ControllerTests
{
    public class PaymentControllerTests
    {
        private readonly Mock<IPaymentService> _mockPaymentSerice;
        private readonly Mock<IShoppingCartService> _mockShoppingCartService;
        private readonly PaymentController _paymentController;

        public PaymentControllerTests()
        {
            _mockPaymentSerice = new Mock<IPaymentService>();
            _mockShoppingCartService = new Mock<IShoppingCartService>();

            _paymentController = new PaymentController(
                _mockPaymentSerice.Object,
                _mockShoppingCartService.Object);
        }

        [Fact]
        public async Task GetInvoiceAsync_Returns_Invoice()
        {
            // Arrange
            var paymentCreateDTO = new PaymentCreateDTO();
            var invoiceStream = new MemoryStream();
            var invoiceContent = new byte[] { 1, 2, 3 };
            await invoiceStream.WriteAsync(invoiceContent, 0, invoiceContent.Length);
            invoiceStream.Seek(0, SeekOrigin.Begin);

            var paymentResult = new PaymentResultDTO
            {
                InvoiceMemoryStream = invoiceStream,
                OrderId = 1
            };

            _mockPaymentSerice
                .Setup(x => x
                    .ExecutePaymentAsync(It.IsAny<PaymentCreateDTO>()))
                .ReturnsAsync(paymentResult);

            // Act
            var response = await _paymentController.GetInvoiceAsync(paymentCreateDTO);

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task PayAsync_Returns_OrderId()
        {
            // Arrange
            var paymentCreateDTO = new PaymentCreateDTO(); // Create a sample DTO object
            var expectedOrderId = 1;

            var paymentResult = new PaymentResultDTO
            {
                OrderId = expectedOrderId
            };

            _mockPaymentSerice
                .Setup(x => x
                    .ExecutePaymentAsync(It.IsAny<PaymentCreateDTO>()))
                .ReturnsAsync(paymentResult);

            // Act
            var response = await _paymentController.PayAsync(paymentCreateDTO);

            // Assert
            Assert.NotNull(response);
            var jsonResult = Assert.IsType<JsonResult<int>>(response);
            var orderId = Assert.IsType<int>(jsonResult.Content);
            Assert.Equal(expectedOrderId, orderId);
        }
    }
}
