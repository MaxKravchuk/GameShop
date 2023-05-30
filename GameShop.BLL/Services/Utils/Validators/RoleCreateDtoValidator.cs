using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.RoleDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class RoleCreateDtoValidator : AbstractValidator<RoleBaseDTO>
    {
        public RoleCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Role`s name cannot be empty");
        }
    }
}
