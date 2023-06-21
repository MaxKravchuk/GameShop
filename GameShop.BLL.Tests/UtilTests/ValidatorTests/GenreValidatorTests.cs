using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class GenreValidatorTests
    {
        private readonly GenreCreateDtoValidator _validationRules;

        public GenreValidatorTests()
        {
            _validationRules = new GenreCreateDtoValidator();
        }

        [Fact]
        public async Task GenreCreateDtoValidator_Validate_ValidInput()
        {
            // Arrange
            var genreCreateDto = new GenreCreateDTO
            {
                Name = "Test",
                ParentGenreId = 1
            };

            // Act
            var result = await _validationRules.ValidateAsync(genreCreateDto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task GenreCreateDtoValidator_Validate_InvalidInput()
        {
            // Arrange
            var genreCreateDto = new GenreCreateDTO
            {
                Name = string.Empty,
                ParentGenreId = 1
            };

            // Act
            var result = await _validationRules.ValidateAsync(genreCreateDto);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
