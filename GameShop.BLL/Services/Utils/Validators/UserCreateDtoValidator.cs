using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.UserDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.NickName)
                .NotEmpty()
                .WithMessage("Nickname cannot be empty");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty");
        }
    }
}
