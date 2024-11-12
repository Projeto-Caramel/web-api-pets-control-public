using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Domain.Entities.Models.Favorites;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Enums.Pets;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services.Favorites;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Favorites
{
    public class FavoritePetsService : BaseService, IFavoritePetsService
    {
        private readonly IAdopterApiService _adopterApiService;

        public FavoritePetsService(IUnitOfWork unitOfWork, IAdopterApiService adopterApiService) : base(unitOfWork)
        {
            _adopterApiService = adopterApiService;
        }

        public async Task<FavoritePets> GetFavoritePetsByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new BusinessException("O campo ID do Usuário é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var user = await _adopterApiService.GetSingleOrDefaultByIdAsync(userId);

            if (user is null)
                throw new BusinessException("Usuário não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.NotFound);

            var favorites = await _unitOfWork.FavoritePets.GetSingleAsync(x => x.UserId == userId);

            if (favorites is not null) return favorites;

            FavoritePets newFavorites = new()
            {
                UserId = userId
            };

            await _unitOfWork.FavoritePets.AddAsync(newFavorites);

            return newFavorites;
        }

        public async Task<FavoritePets> AddFavoritePetForUser(string userId, string petId)
        {
            var favorites = await GetFavoritePetsByUserAsync(userId);

            var pet = await _unitOfWork.Pets.GetSingleAsync(petId) ??
                throw new BusinessException("Pet não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.NotFound);

            favorites.Pets.Add(pet);

            _unitOfWork.FavoritePets.Update(favorites);

            return favorites;
        }

        public async Task<FavoritePets> RemoveFavoritePetForUser(string userId, string petId)
        {
            var favorites = await GetFavoritePetsByUserAsync(userId);

            var pet = await _unitOfWork.Pets.GetSingleAsync(petId) ??
                throw new BusinessException("Pet não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.NotFound);

            favorites.Pets.RemoveAll(p => p.Id == pet.Id);

            _unitOfWork.FavoritePets.Update(favorites);

            return favorites;
        }

    }
}
