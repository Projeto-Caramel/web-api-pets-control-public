using Caramel.Pattern.Services.Domain.Entities.Models.Favorites;
using Caramel.Pattern.Services.Domain.Entities.Models.Pets;
using Microsoft.EntityFrameworkCore;

namespace Caramel.Pattern.Services.Infra.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> contextOptions) : base(contextOptions)
        { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetGalleryImage> Galleries { get; set; }
        public DbSet<FavoritePets> Favorites { get; set; }
    }
}
