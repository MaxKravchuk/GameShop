using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.GameDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class GameCreateDTOValidator : AbstractValidator<GameCreateDTO>
    {
        public GameCreateDTOValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("Game name cannot be empty");

            RuleFor(g => g.Description)
                .NotEmpty()
                .WithMessage("Game description cannot be empy");

            RuleFor(g => g.Key)
                .NotEmpty()
                .WithMessage("Game key cannot be empy");

            RuleFor(g => g.GenresId)
                .NotEmpty()
                .WithMessage("Game cannot be without genres");

            RuleFor(g => g.PlatformTypeId)
                .NotEmpty()
                .WithMessage("Game cannot be without platform types");
        }
    }
}
