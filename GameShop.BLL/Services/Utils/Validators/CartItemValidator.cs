using FluentValidation;
using GameShop.BLL.DTO.RedisDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class CartItemValidator : AbstractValidator<CartItemDTO>
    {
        public CartItemValidator()
        {
            RuleFor(x => x.GameKey)
                .NotEmpty()
                .WithMessage("Game key cannot be empty");

            RuleFor(x => x.GameName)
                .NotEmpty()
                .WithMessage("Game name cannot be empty");

            RuleFor(x => x.GamePrice)
                .GreaterThan(0)
                .WithMessage("Game price cannot be less than 0 or empty");
        }
    }
}
