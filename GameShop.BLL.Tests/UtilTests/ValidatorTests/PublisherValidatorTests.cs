using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace BLL.Test.UtilTests.ValidatorTests
{
    public class PublisherValidatorTests
    {
        private readonly PublisherCreateDTOValidator _validationRules;

        public PublisherValidatorTests()
        {
            _validationRules = new PublisherCreateDTOValidator();
        }

        [Fact]
        public void PublisherCreateDTOValidator_Validate_ValidInput_ShouldPass()
        {
            // Arrange
            var publisherCreateDTO = new PublisherCreateDTO
            {
                CompanyName = "test",
                Description = "test",
                HomePage = "tst"
            };

            // Act
            var result = _validationRules.Validate(publisherCreateDTO);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "test", "test")]
        [InlineData("test", "", "test")]
        [InlineData("test", "test", "")]
        public void PublisherCreateDTOValidator_Validate_InvalidInput_ShouldFail(
            string companyName, string desc, string homePage)
        {
            // Arrange
            var publisherCreateDTO = new PublisherCreateDTO
            {
                CompanyName = companyName,
                Description = desc,
                HomePage = homePage
            };

            // Act
            // Act
            var result = _validationRules.Validate(publisherCreateDTO);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
