using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GameShop.BLL.DTO.RoleDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class RoleValidatorTests
    {
        private readonly RoleCreateDtoValidator _validationRules;

        public RoleValidatorTests()
        {
            _validationRules = new RoleCreateDtoValidator();
        }

        [Fact]
        public async Task RoleCreateDtoValidator_Validate_ValidInput()
        {
            // Arrange
            var roleCreateDto = new RoleCreateDTO
            {
                Name = "Test"
            };

            // Act
            var result = await _validationRules.ValidateAsync(roleCreateDto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task RoleCreateDtoValidator_Validate_InvalidInput()
        {
            // Arrange
            var roleCreateDto = new RoleCreateDTO
            {
                Name = string.Empty
            };

            // Act
            var result = await _validationRules.ValidateAsync(roleCreateDto);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
