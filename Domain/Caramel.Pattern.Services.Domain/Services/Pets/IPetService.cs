using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;

namespace Caramel.Pattern.Services.Domain.Services.Pets
{
    public interface IPetService
    {
        Task<Pet> GetSingleOrDefaultByIdAsync(string id);
        Task<PetStatus> GetPetStatusAsync(string id);
        Task<IEnumerable<Pet>> FetchAsync(string partnerId);
        Task<IEnumerable<Pet>> FetchByFilterAsync(string partnerId, PetFilter filter);
        Task<IEnumerable<Pet>> FetchByRangeAsync(List<string> ids);
        Task<IEnumerable<Pet>> FetchAllAsync();
        Task<Pet> RegisterAsync(Pet entity, string base64Image);
        Task<Pet> UpdateAsync(Pet entity, string base64Image);
        Task<PetStatus> UpdateStatusAsync(string id, PetStatus status);
        Task DeleteAsync(string id);
        Task<string> GetImageBase64(string id);
    }
}
