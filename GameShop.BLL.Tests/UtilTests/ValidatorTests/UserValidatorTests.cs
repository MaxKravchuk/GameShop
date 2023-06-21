using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class UserValidatorTests
    {
        private readonly UserCreateDtoValidator _validationRules;

        public UserValidatorTests()
        {
            _validationRules = new UserCreateDtoValidator();
        }

        [Fact]
        public async Task UserCreateDtoValidator_Validate_ValidInput()
        {
            // Arrange
            var userCreateDto = new UserCreateDTO
            {
                NickName = "Test",
                Password = "Test"
            };

            // Act
            var result = await _validationRules.ValidateAsync(userCreateDto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task UserCreateDtoValidator_Validate_InvalidInput()
        {
            // Arrange
            var userCreateDto = new UserCreateDTO
            {
                NickName = string.Empty,
                Password = string.Empty
            };

            // Act
            var result = await _validationRules.ValidateAsync(userCreateDto);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
