using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.GenreDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class GenreCreateDtoValidator : AbstractValidator<GenreCreateDTO>
    {
        public GenreCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Genre name cannot be empty");
        }
    }
}
