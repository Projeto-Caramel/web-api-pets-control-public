using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Caramel.Pattern.Services.Api.Configurators
{
    public static class SwaggerConfigurator
    {
        public static void ConfigureSwagger(this IServiceCollection services, string environment)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"API de Autenticação e Autorização - {environment}",
                    Version = "v1",
                    Description = "Exemplo de API Auth a ser seguida nos Microsserviços do Projeto Caramel (TCC - EC 2024)."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Acesso protegido utilizando o accessToken obtido em \"api/Authenticate/login\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}
