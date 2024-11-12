using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Validators
{
    [ExcludeFromCodeCoverage]
    public class PetGalleryImageValidator : AbstractValidator<PetGalleryImage>
    {
        public PetGalleryImageValidator()
        {
            RuleFor(x => x.PetId)
                .NotEmpty()
                .WithMessage("O Pet ID é obrigatório.");
        }
    }
}
