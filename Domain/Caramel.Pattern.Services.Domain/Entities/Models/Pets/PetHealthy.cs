using Caramel.Pattern.Services.Domain.Enums.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Pets
{
    [ExcludeFromCodeCoverage]
    public class PetHealthy
    {
        public CastratedStatus Castrated { get; set; }
        public VaccinatedStatus Vaccinated { get; set; }
    }
}
