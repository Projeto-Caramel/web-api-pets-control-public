using Caramel.Pattern.Services.Application.Services;
using Caramel.Pattern.Services.Application.Services.Dashboard;
using Caramel.Pattern.Services.Application.Services.Favorites;
using Caramel.Pattern.Services.Application.Services.Pets;
using Caramel.Pattern.Services.Application.Services.Security;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Dashboard;
using Caramel.Pattern.Services.Domain.Services.Favorites;
using Caramel.Pattern.Services.Domain.Services.Gallery;
using Caramel.Pattern.Services.Domain.Services.Pets;
using Caramel.Pattern.Services.Domain.Services.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationDependency
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IDashboardService, DashboardDataService>();
            services.AddScoped<IPetGalleryService, PetGalleyService>();
            services.AddScoped<IFavoritePetsService, FavoritePetsService>();
            services.AddScoped<IBucketService, BucketService>();

            return services;
        }
    }
}
