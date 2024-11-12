using Caramel.Pattern.Services.Domain.Entities.Models.Pets;

namespace Caramel.Pattern.Services.Domain.Services.Gallery
{
    public interface IPetGalleryService
    {
       Task<IEnumerable<PetGalleryImage>> GetGalleryByPetIdAsync(string id);

       Task<PetGalleryImage> RegisterAsync(PetGalleryImage entity, string base64Image);

       Task DeleteAsync(string id);
    }
}
