using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class PlatformTypeValidatorTests
    {
        private readonly PlatformTypeCreateDtoValidator _validationRules;

        public PlatformTypeValidatorTests()
        {
            _validationRules = new PlatformTypeCreateDtoValidator();
        }

        [Fact]
        public async Task PlatformTypeCreateDtoValidator_Validate_ValidInput()
        {
            // Arrange
            var platformTypeCreateDto = new PlatformTypeCreateDTO
            {
                Type = "Test"
            };

            // Act
            var result = await _validationRules.ValidateAsync(platformTypeCreateDto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task PlatformTypeCreateDtoValidator_Validate_InvalidInput()
        {
            // Arrange
            var platformTypeCreateDto = new PlatformTypeCreateDTO
            {
                Type = string.Empty
            };

            // Act
            var result = await _validationRules.ValidateAsync(platformTypeCreateDto);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
