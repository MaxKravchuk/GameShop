using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace WebApi.Test.ControllerTests
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
    }
}
