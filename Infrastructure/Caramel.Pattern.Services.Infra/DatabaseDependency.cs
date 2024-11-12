using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Infra.Context;
using Caramel.Pattern.Services.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Infra
{
    [ExcludeFromCodeCoverage]
    public static class DatabaseDependency
    {
        public static IServiceCollection AddDatabaseModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<MongoDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IPetGalleryRepository, PetGalleryRepository>();
            services.AddScoped<IFavoritePetsRepository, FavoritePetsRepository>();

            return services;
        }
    }
}
