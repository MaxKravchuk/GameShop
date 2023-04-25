using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.PublisherDTOs;

namespace GameShop.BLL.Services.Utils.Validators
{
    public class PublisherCreateDTOValidator : AbstractValidator<PublisherCreateDTO>
    {
        public PublisherCreateDTOValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Company name cannot be empy");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description cannot be empy");

            RuleFor(x => x.HomePage)
                .NotEmpty()
                .WithMessage("Home page cannot be empy");
        }
    }
}
