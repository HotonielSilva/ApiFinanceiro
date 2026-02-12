using ApiFinanceiro.Services.Implementations;
using ApiFinanceiro.Services.Interfaces;

namespace ApiFinanceiro.Configurations.DependencyInjection;

public static class ApplicationServicesExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // ⚙️ Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IProgressoService, ProgressoService>();

        // 📦 Repository
    }
}
