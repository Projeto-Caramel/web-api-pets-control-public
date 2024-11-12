namespace Caramel.Pattern.Services.Domain.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPetRepository Pets { get; }
        IPetGalleryRepository PetImages { get; }
        IFavoritePetsRepository FavoritePets { get; }
    }
}
