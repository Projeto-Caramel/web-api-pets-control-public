using Caramel.Pattern.Services.Domain.Entities.Models.Favorites;

namespace Caramel.Pattern.Services.Domain.Services.Favorites
{
    public interface IFavoritePetsService
    {
        Task<FavoritePets> GetFavoritePetsByUserAsync(string userId);
        Task<FavoritePets> AddFavoritePetForUser(string userId, string petId);
        Task<FavoritePets> RemoveFavoritePetForUser(string userId, string petId);
    }
}
