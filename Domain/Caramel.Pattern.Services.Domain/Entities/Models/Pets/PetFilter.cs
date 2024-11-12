using Caramel.Pattern.Services.Domain.Enums.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Pets
{
    [ExcludeFromCodeCoverage]
    public record PetFilter()
    {
        public string Name { get; set; }
        public CastratedStatus Castrated { get; set; }
        public VaccinatedStatus Vaccinated { get; set; }
        public AgeType Age { get; set; }
        public PetStatus Status { get; set; }
        public PetSex Sex { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
