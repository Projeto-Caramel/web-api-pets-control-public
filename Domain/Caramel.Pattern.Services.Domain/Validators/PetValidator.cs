using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Validators
{
    [ExcludeFromCodeCoverage]
    public class PetValidator : AbstractValidator<Pet>
    {
        public PetValidator()
        {
            RuleFor(x => x.PartnerId)
                .NotEmpty()
                .WithMessage("O Partner ID é obrigatório.");

            RuleFor(x => x.Info)
                .NotNull()
                .WithMessage("O campo Info é obrigatório.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Info.Name)
                        .NotEmpty()
                        .WithMessage("O campo Nome é obrigatório.");

                    RuleFor(x => x.Info.Description)
                        .NotEmpty()
                        .WithMessage("O campo Descrição é obrigatório.");

                    RuleFor(x => x.Info.Status)
                        .IsInEnum()
                        .WithMessage("O Status deve ser um valor entre 0 e 4.");
                });

            RuleFor(x => x.Healthy)
                .NotNull()
                .WithMessage("O campo Healthy é obrigatório.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Healthy.Castrated)
                        .IsInEnum()
                        .WithMessage("O Castrated deve ser um valor entre 0 e 2.");

                    RuleFor(x => x.Healthy.Vaccinated)
                        .IsInEnum()
                        .WithMessage("O Vaccinated deve ser um valor entre 0 e 2.");
                });

            RuleFor(x => x.Caracteristics)
                .NotNull()
                .WithMessage("O campo Caracteristics é obrigatório.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Caracteristics.Sex)
                        .IsInEnum()
                        .WithMessage("O Sexo deve ser um valor entre 0 e 2.");

                    RuleFor(x => x.Caracteristics.Coat)
                        .IsInEnum()
                        .WithMessage("O Coat deve ser um valor entre 0 e 4.");

                    RuleFor(x => x.Caracteristics.EnergyLevel)
                        .IsInEnum()
                        .WithMessage("O EnergyLevel deve ser um valor entre 0 e 3.");

                    RuleFor(x => x.Caracteristics.Size)
                        .IsInEnum()
                        .WithMessage("O Size deve ser um valor entre 0 e 3.");

                    RuleFor(x => x.Caracteristics.StimulusLevel)
                        .IsInEnum()
                        .WithMessage("O StimulusLevel deve ser um valor entre 0 e 3.");

                    RuleFor(x => x.Caracteristics.Temperament)
                        .IsInEnum()
                        .WithMessage("O Temperament deve ser um valor entre 0 e 3.");

                    RuleFor(x => x.Caracteristics.ChildLove)
                        .IsInEnum()
                        .WithMessage("O ChildLove deve ser um valor entre 0 e 3.");

                    RuleFor(x => x.Caracteristics.AnimalsSocialization)
                        .IsInEnum()
                        .WithMessage("O AnimalsSocialization deve ser um valor entre 0 e 3.");

                    RuleFor(x => x.Caracteristics.SpecialNeeds)
                        .IsInEnum()
                        .WithMessage("O SpecialNeeds deve ser um valor entre 0 e 3.");

                    RuleFor(x => x.Caracteristics.Shedding)
                        .IsInEnum()
                        .WithMessage("O Shedding deve ser um valor entre 0 e 3.");
                });
        }
    }
}
