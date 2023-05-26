using FluentAssertions;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class CartItemValidatorTests
    {
        private readonly CartItemValidator _validationRules;

        public CartItemValidatorTests()
        {
            _validationRules = new CartItemValidator();
        }

        [Fact]
        public void CartItemValidator_Validate_ValidInput()
        {
            // Arrange
            var cartItem = new CartItemDTO
            {
                GameKey = "1234",
                GameName = "Test Game",
                GamePrice = 9.99m
            };

            // Act
            var result = _validationRules.Validate(cartItem);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "Test Game", 9.99, "Game key cannot be empty")]
        [InlineData("1234", "", 9.99, "Game name cannot be empty")]
        [InlineData("1234", "Test Game", -9.99, "Game price cannot be less than 0 or empty")]
        [InlineData("", "", 0, "Game key cannot be empty")]
        public void CartItemValidator_Validate_InvalidInput(string gameKey, string gameName, decimal gamePrice, string errorMessage)
        {
            // Arrange
            var cartItem = new CartItemDTO
            {
                GameKey = gameKey,
                GameName = gameName,
                GamePrice = gamePrice
            };

            // Act
            var result = _validationRules.Validate(cartItem);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == errorMessage);
        }
    }
}
