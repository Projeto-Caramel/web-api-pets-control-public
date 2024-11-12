﻿using Caramel.Pattern.Integrations.UsersControl.ApiServices;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Integrations.UsersControl
{
    public static class UsersControlDependency
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddUsersControlModule(this IServiceCollection services)
        {
            services.AddScoped<IPartnerApiService, PartnerApiService>();
            services.AddScoped<IAdopterApiService, AdoptersApiService>();

            return services;
        }
    }
}
