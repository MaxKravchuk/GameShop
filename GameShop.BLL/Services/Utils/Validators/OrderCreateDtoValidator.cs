using FluentValidation;
using GameShop.BLL.DTO.OrderDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(g => g.CustomerID)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Customer ID cannot be empty");

            RuleFor(g => g.OrderedAt)
                .NotEmpty()
                .WithMessage("Time cannot be empty");
        }
    }
}
