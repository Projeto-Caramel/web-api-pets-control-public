using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request
{
    [ExcludeFromCodeCoverage]
    public record PetRequest(string PartnerId, PetInfoRequest Info, PetHealthy Healthy, PetCharacteristics Caracteristics)
    {
    }

    public class PetInfoRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BirthDate { get; set; }
        public PetStatus Status { get; set; }
        public string Base64Image { get; set; }
    }
}
