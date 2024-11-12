using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request
{
    [ExcludeFromCodeCoverage]
    public record PetGalleryImageRequest(string PetId, string Base64Image);
}
