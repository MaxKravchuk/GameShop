using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.PlatformTypeDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class PlatformTypeCreateDtoValidator : AbstractValidator<PlatformTypeCreateDTO>
    {
        public PlatformTypeCreateDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Platform type name cannot be empty");
        }
    }
}
