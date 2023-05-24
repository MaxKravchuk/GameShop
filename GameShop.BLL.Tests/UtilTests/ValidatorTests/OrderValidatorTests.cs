using System;
using FluentValidation.TestHelper;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace GameShop.BLL.Tests.UtilTests.ValidatorTests
{
    public class OrderValidatorTests
    {
        private readonly OrderCreateDtoValidator _validationRules;

        public OrderValidatorTests()
        {
            _validationRules = new OrderCreateDtoValidator();
        }

        [Fact]
        public void Given_Empty_CustomerID_Should_Return_Error()
        {
            var order = new OrderCreateDTO
            {
                Strategy = "s",
                OrderedAt = DateTime.UtcNow,
            };

            var result = _validationRules.TestValidate(order);

            result.ShouldHaveValidationErrorFor(o => o.CustomerID)
                  .WithErrorMessage("Customer ID cannot be empty");
        }

        [Fact]
        public void Given_Empty_OrderedAt_Should_Return_Error()
        {
            var order = new OrderCreateDTO
            {
                CustomerID = 1,
                OrderedAt = DateTime.MinValue, // or any other invalid value
            };

            var result = _validationRules.TestValidate(order);

            result.ShouldHaveValidationErrorFor(o => o.OrderedAt)
                  .WithErrorMessage("Time cannot be empty");
        }
    }
}
