using FluentValidation;
using GameShop.BLL.DTO.GameDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class GameCreateDtoValidator : AbstractValidator<GameCreateDTO>
    {
        public GameCreateDtoValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("Game name cannot be empty");

            RuleFor(g => g.Description)
                .NotEmpty()
                .WithMessage("Game description cannot be empty");

            RuleFor(g => g.Key)
                .NotEmpty()
                .WithMessage("Game key cannot be empty");

            RuleFor(g => g.Price)
                .NotEmpty()
                .WithMessage("Game cannot be free");

            RuleFor(g => g.UnitsInStock)
                .NotEmpty()
                .WithMessage("Game must have any quantity");

            RuleFor(g => g.Discontinued)
                .NotEmpty()
                .WithMessage("Game must have a discontinued state");
        }
    }
}
