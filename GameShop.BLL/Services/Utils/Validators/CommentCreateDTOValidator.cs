using FluentValidation;
using GameShop.BLL.DTO.CommentDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDTO>
    {
        public CommentCreateDtoValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("Comment must have author name");

            RuleFor(g => g.Body)
                .NotEmpty()
                .WithMessage("Comment body cannot be empty");

            RuleFor(g => g.GameKey)
                .NotEmpty()
                .WithMessage("Comment must have game key");
        }
    }
}
