using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.GameDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class CommentCreateDTOValidator : AbstractValidator<CommentCreateDTO>
    {
        public CommentCreateDTOValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("Comment must have author name");

            RuleFor(g => g.Body)
                .NotEmpty()
                .WithMessage("Comment body cannot be empy");

            RuleFor(g => g.GameKey)
                .NotEmpty()
                .WithMessage("Comment must have game key");
        }
    }
}
