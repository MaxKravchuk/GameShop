using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using GameShop.BLL.DTO.OrderDetailsDTOs;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Utils.Validators;
using Xunit;

namespace BLL.Test.UtilTests.ValidatorTests
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
                CustomerID = 0, // or any other invalid value
                OrderedAt = DateTime.UtcNow,
                ListOfOrderDetails = new List<OrderDetailsCreateDTO>()
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
                ListOfOrderDetails = new List<OrderDetailsCreateDTO>()
            };

            var result = _validationRules.TestValidate(order);

            result.ShouldHaveValidationErrorFor(o => o.OrderedAt)
                  .WithErrorMessage("Time cannot be empty");
        }

        [Fact]
        public void Given_Empty_ListOfOrderDetails_Should_Return_Error()
        {
            var order = new OrderCreateDTO
            {
                CustomerID = 1,
                OrderedAt = DateTime.UtcNow,
                ListOfOrderDetails = null // or an empty list
            };

            var result = _validationRules.TestValidate(order);

            result.ShouldHaveValidationErrorFor(o => o.ListOfOrderDetails)
                  .WithErrorMessage("Order cannot be empty");
        }
    }
}
