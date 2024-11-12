using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request
{
    [ExcludeFromCodeCoverage]
    public class GetPetsFilteredRequest
    {
        public Pagination Pagination { get; set; }
        public PetFilter PetFilter { get; set; }
    }
}
