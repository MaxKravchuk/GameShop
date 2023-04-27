using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.RedisDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class CartItemValidator : AbstractValidator<CartItem>
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
                .NotEmpty()
                .WithMessage("Game price cannot be less than 0 or empy");
        }
    }
}
