using System.Collections.Generic;
using FluentAssertions;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class GameValidatorTests
    {
        private readonly GameCreateDtoValidator _validationRules;

        public GameValidatorTests()
        {
            _validationRules = new GameCreateDtoValidator();
        }

        [Fact]
        public void GameCreateDTOValidator_ShouldNotHaveErrors()
        {
            // Arrange
            var game = new GameCreateDTO
            {
                Name = "Test Game",
                Description = "This is a test game",
                Key = "TEST",
                GenresId = new List<int> { 1, 2 },
                PlatformTypeId = new List<int> { 1, 2 },
                Price = 9.99m,
                UnitsInStock = 10,
                Discontinued = true
            };

            // Act
            var result = _validationRules.Validate(game);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData("", "Desc", "1111", 10.10, 1, false)]
        [InlineData("Game", "", "1111", 10, 1, false)]
        [InlineData("Game", "Desc", "", 10, 1, false)]
        [InlineData("Game", "Desc", "1111", default, 1, false)]
        [InlineData("Game", "Desc", "1111", 10, default, false)]
        [InlineData("Game", "Desc", "1111", 10, 1, default)]
        public void GameCreateDTOValidator_Validate_InvalidInput_ShouldFail(
            string name, string desc, string key, decimal price, short unitsInStock, bool discontinued)
        {
            // Arrange
            var commentCreateDTO = new GameCreateDTO
            {
                Name = name,
                Description = desc,
                Key = key,
                GenresId = new List<int> { 1, 2 },
                PlatformTypeId = new List<int> { 1, 2 },
                Price = price,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued
            };

            // Act
            var result = _validationRules.Validate(commentCreateDTO);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void GameCreateDTOValidator_Validate_InvalidGenresList_ShouldFail()
        {
            // Arrange
            var gameCreateDTO = new GameCreateDTO
            {
                Name = "Game",
                Description = "Desc",
                Key = "1111",
                GenresId = null,
                PlatformTypeId = new List<int> { 1, 2 },
                Price = 10m,
                UnitsInStock = 1,
                Discontinued = false
            };

            // Act
            var result = _validationRules.Validate(gameCreateDTO);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void GameCreateDTOValidator_Validate_InvalidPlatformTypesList_ShouldFail()
        {
            // Arrange
            var gameCreateDTO = new GameCreateDTO
            {
                Name = "Game",
                Description = "Desc",
                Key = "1111",
                GenresId = new List<int> { 1, 2 },
                PlatformTypeId = null,
                Price = 10m,
                UnitsInStock = 1,
                Discontinued = false
            };

            // Act
            var result = _validationRules.Validate(gameCreateDTO);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
