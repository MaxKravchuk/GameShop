using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace GameShop.WebApi.Tests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();

            _orderController = new OrderController(_mockOrderService.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_Returns_NewOrder()
        {
            // Arrange
            var orderCreateDTO = new OrderCreateDTO();

            _mockOrderService
                .Setup(x => x
                    .CreateOrderAsync(It.IsAny<OrderCreateDTO>()))
                .ReturnsAsync(1);

            // Act
            var response = await _orderController.CreateOrderAsync(orderCreateDTO);

            // Assert
            Assert.NotNull(response);
            var okResult = Assert.IsType<OkNegotiatedContentResult<int>>(response);
            var newOrder = Assert.IsType<int>(okResult.Content);
            Assert.Equal(1, newOrder);
        }

        [Fact]
        public async Task GetAllAsync_WithValidRole_ReturnsJsonResult()
        {
            // Arrange
            var orders = new List<OrderReadListDTO>
            {
                new OrderReadListDTO()
            };

            _mockOrderService
                .Setup(s => s
                    .GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderController.GetAllAsync();

            // Assert
            var jsonResult = Assert.IsType<JsonResult<IEnumerable<OrderReadListDTO>>>(result);
            _mockOrderService.Verify(s => s.GetAllOrdersAsync(), Times.Once);
        }
    }
}
