using Caramel.Pattern.Services.Domain.Entities.Models.Favorites;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Infra.Context;

namespace Caramel.Pattern.Services.Infra.Repositories
{
    public class FavoritePetsRepository(MongoDbContext context) : BaseRepository<FavoritePets, string>(context,"favorites-data"), IFavoritePetsRepository
    {

    }
}

