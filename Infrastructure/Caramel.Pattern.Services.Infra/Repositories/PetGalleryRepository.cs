using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Infra.Context;

namespace Caramel.Pattern.Services.Infra.Repositories
{
    public class PetGalleryRepository(MongoDbContext context) : BaseRepository<PetGalleryImage, string>(context,"pets-gallery"), IPetGalleryRepository
    {
    }
}
