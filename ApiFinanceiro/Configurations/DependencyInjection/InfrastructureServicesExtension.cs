using ApiFinanceiro.Configurations.Swagger;
using ApiFinanceiro.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Configurations.DependencyInjection;

public static class InfrastructureServicesExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ✅ Registra os Controllers
        services.AddControllers();

        // 🔧 Banco de dados
        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Scoped);

        // 🌐 CORS
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.SetIsOriginAllowed(origin => true)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        // 📄 Swagger
        services.AddSwaggerConfig(); // ✅ definido em SwaggerConfigExtension.cs

    }
}
