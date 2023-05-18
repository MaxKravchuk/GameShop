using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Repository.Interfaces.Utils;
using Moq;
using Xunit;

namespace GameShop.BLL.Tests.ServiceTests
{
    public class ShoppingCartServiceTests : IDisposable
    {
        private readonly Mock<IRedisProvider<CartItemDTO>> _mockRedisProvider;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IValidator<CartItemDTO>> _mockValidator;
        private readonly ShoppingCartService _shoppingCartService;

        private bool _disposed;

        public ShoppingCartServiceTests()
        {
            _mockRedisProvider = new Mock<IRedisProvider<CartItemDTO>>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<CartItemDTO>>();

            _shoppingCartService = new ShoppingCartService(
                _mockRedisProvider.Object,
                _mockLogger.Object,
                _mockValidator.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task AddCartItemAsync_ExistingItem()
        {
            // Arrange
            var existingCartItem = new CartItemDTO { GameKey = "existing", Quantity = 1 };
            var newCartItem = new CartItemDTO { GameKey = "existing", Quantity = 1 };

            _mockRedisProvider
                .Setup(x => x
                    .GetValueAsync(It.IsAny<string>(), "existing"))
                .ReturnsAsync(existingCartItem);

            // Act
            await _shoppingCartService.AddCartItemAsync(newCartItem);

            // Assert
            _mockRedisProvider.Verify(x => x.SetValueToListAsync(It.IsAny<string>(), "existing", existingCartItem), Times.Once);
            _mockLogger.Verify(x => x.LogInfo("Item with key existing updated"), Times.Once);
        }

        [Fact]
        public async Task AddCartItemAsync_NewItem()
        {
            // Arrange
            var newCartItem = new CartItemDTO { GameKey = "new", Quantity = 1 };

            _mockRedisProvider
                .Setup(x => x
                    .GetValueAsync(It.IsAny<string>(), "new"))
                .ReturnsAsync(default(CartItemDTO));

            // Act
            await _shoppingCartService.AddCartItemAsync(newCartItem);

            // Assert
            _mockRedisProvider.Verify(x => x.SetValueToListAsync(It.IsAny<string>(), "new", newCartItem), Times.Once);
            _mockLogger.Verify(x => x.LogInfo("New item with key new added to cart"), Times.Once);
        }

        [Fact]
        public async Task GetCartItemsAsync()
        {
            // Arrange
            var cartItems = new List<CartItemDTO>
            {
                new CartItemDTO
                {
                    GameKey = "item1", Quantity = 1
                },
                new CartItemDTO
                {
                    GameKey = "item2", Quantity = 2
                }
            };

            _mockRedisProvider
                .Setup(x => x
                    .GetValuesAsync(It.IsAny<string>()))
                .ReturnsAsync(cartItems);

            // Act
            var result = await _shoppingCartService.GetCartItemsAsync();

            // Assert
            _mockRedisProvider.Verify(x => x.GetValuesAsync(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(x => x.LogInfo($"List of items returned with array length of {cartItems.Count}"), Times.Once);
            Assert.Equal(cartItems, result);
        }

        [Fact]
        public async Task DeletItemFromList_ExistingItem_QuantityGreaterThan1()
        {
            // Arrange
            var existingCartItem = new CartItemDTO { GameKey = "existing", Quantity = 2 };

            _mockRedisProvider
                .Setup(x => x
                    .GetValueAsync(It.IsAny<string>(), "existing"))
                .ReturnsAsync(existingCartItem);

            // Act
            await _shoppingCartService.DeleteItemFromListAsync("existing");

            // Assert
            _mockRedisProvider.Verify(x => x.SetValueToListAsync(It.IsAny<string>(), "existing", existingCartItem), Times.Once);
            _mockLogger.Verify(x => x.LogInfo("Item with game key existing is deleted"), Times.Once);
        }

        [Fact]
        public async Task DeleteItemFromList_ExistingCartItemWithQuantityGreaterThanOne_DecrementsQuantityAndDoesNotDelete()
        {
            // Arrange
            var existingCartItem = new CartItemDTO
            {
                GameKey = "gamekey",
                Quantity = 2
            };

            _mockRedisProvider
                .Setup(x => x
                    .GetValueAsync("CartItems", existingCartItem.GameKey))
                .ReturnsAsync(existingCartItem);

            // Act
            await _shoppingCartService.DeleteItemFromListAsync(existingCartItem.GameKey);

            // Assert
            existingCartItem.Quantity -= 1;
            _mockRedisProvider.Verify(x => x.DeleteItemFromListAsync("CartItems", existingCartItem.GameKey), Times.Never);
            _mockRedisProvider.Verify(x => x.SetValueToListAsync("CartItems", existingCartItem.GameKey, existingCartItem), Times.Once);
            _mockLogger.Verify(x => x.LogInfo($"Item with game key {existingCartItem.GameKey} is deleted"), Times.Once);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _mockRedisProvider.Invocations.Clear();
                _mockValidator.Invocations.Clear();
                _mockLogger.Invocations.Clear();
            }

            _disposed = true;
        }
    }
}
