using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Controllers;
using Moq;
using Xunit;

namespace WebApi.Test.ControllerTests
{
    public class ShoppingCartControllerTests
    {
        private readonly Mock<IShoppingCartService> _mockShoppingCartService;
        private readonly ShoppingCartController _shoppingCartController;

        public ShoppingCartControllerTests()
        {
            _mockShoppingCartService = new Mock<IShoppingCartService>();
            _shoppingCartController = new ShoppingCartController(_mockShoppingCartService.Object);
        }

        [Fact]
        public async Task AddGameToCart_ShouldAddNewCartItemToCache()
        {
            // Arrange
            var cartItem = new CartItemDTO();

            // Act
            var result = await _shoppingCartController.AddGameToCartAsync(cartItem);

            // Assert
            _mockShoppingCartService.Verify(x => x.AddCartItemAsync(cartItem), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetCartItemAsync_ShouldReturnListFromCache()
        {
            // Arrange
            var cartItems = new List<CartItemDTO> { new CartItemDTO() };

            _mockShoppingCartService
                .Setup(x => x
                    .GetCartItemsAsync())
                .ReturnsAsync(cartItems);

            // Act
            var actionResult = await _shoppingCartController.GetGamesFromCartAsync();

            // Assert
            _mockShoppingCartService.Verify(x => x.GetCartItemsAsync(), Times.Once);
            Assert.IsType<JsonResult<IEnumerable<CartItemDTO>>>(actionResult);
            var jsonResult = (JsonResult<IEnumerable<CartItemDTO>>)actionResult;
            Assert.Equal(cartItems, jsonResult.Content);
        }

        [Fact]
        public async Task DeleteGameFromCart_ShouldDeleteCacheItem()
        {
            // Arrange
            var gameKey = "test";

            // Act
            var result = await _shoppingCartController.DeleteGameFromCartAsync(gameKey);

            // Assert
            _mockShoppingCartService.Verify(x => x.DeletItemFromListAsync(gameKey), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}
