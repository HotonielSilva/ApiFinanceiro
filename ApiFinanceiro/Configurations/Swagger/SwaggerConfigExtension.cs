using ApiFinanceiro.Constants;
using Microsoft.OpenApi.Models;

namespace ApiFinanceiro.Configurations.Swagger;

public static class SwaggerConfigExtension
{
    /// <summary>
    /// Adiciona e configura o Swagger com suporte a autenticação JWT e enums como texto.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <returns>Serviços atualizados com Swagger.</returns>
    public static void AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = MensagensSistema.SwaggerTitulo,
                Description = MensagensSistema.SwaggerDescricao,
                Contact = new OpenApiContact() { Name = "Coris Brasil", Email = "contato@coris.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri(MensagensSistema.SwaggerLicense) }
            });

            // Configuração do esquema de segurança usando as constantes de Segurança
            c.AddSecurityDefinition(MensagensSistema.SecuritySchemeName, new OpenApiSecurityScheme
            {
                Description = MensagensSistema.SecurityDescription,
                Name = MensagensSistema.SecurityHeaderName,
                BearerFormat = MensagensSistema.SecurityBearerFormat,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = MensagensSistema.SecuritySchemeName
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = MensagensSistema.SecuritySchemeName
                        }
                    },
                    Array.Empty<string>()
                }
            });

            c.UseInlineDefinitionsForEnums();
        });
    }
}
