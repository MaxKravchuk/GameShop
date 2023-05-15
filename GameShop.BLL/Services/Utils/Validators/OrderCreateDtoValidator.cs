using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.OrderDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(g => g.CustomerID)
                .NotNull()
                .WithMessage("Customer ID cannot be empty");

            RuleFor(g => g.OrderedAt)
                .NotEmpty()
                .WithMessage("Time cannot be empty");

            RuleFor(g => g.Strategy)
                .NotEmpty()
                .WithMessage("Strategy cannot be empty");

            RuleFor(g => g.IsPaymentSuccessful)
                .NotEmpty()
                .WithMessage("IsPaymentSuccessful cannot be empty");
        }
    }
}
