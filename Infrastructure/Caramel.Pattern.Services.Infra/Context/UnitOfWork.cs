using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;

namespace Caramel.Pattern.Services.Infra.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoDbContext _context;
        public IPetRepository Pets { get; }
        public IPetGalleryRepository PetImages { get; }
        public IFavoritePetsRepository FavoritePets { get; }

        public UnitOfWork(
            MongoDbContext context,
            IPetRepository pets,
            IPetGalleryRepository gallery,
            IFavoritePetsRepository favorites
            )
        {
            _context = context;
            Pets = pets;
            PetImages = gallery;
            FavoritePets = favorites;
        }
    }
}
