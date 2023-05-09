using FluentValidation;
using GameShop.BLL.DTO.PublisherDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class PublisherCreateDtoValidator : AbstractValidator<PublisherCreateDTO>
    {
        public PublisherCreateDtoValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Company name cannot be empty");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description cannot be empty");

            RuleFor(x => x.HomePage)
                .NotEmpty()
                .WithMessage("Home page cannot be empty");
        }
    }
}
