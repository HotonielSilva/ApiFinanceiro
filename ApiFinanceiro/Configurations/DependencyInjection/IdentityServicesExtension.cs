using ApiFinanceiro.Configurations.Identity;
using ApiFinanceiro.Configurations.Jwt;
using ApiFinanceiro.Context;
using ApiFinanceiro.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ApiFinanceiro.Configurations.DependencyInjection;

public static class IdentityServicesExtension
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddRoles<IdentityRole>()
        .AddSignInManager<SignInManager<ApplicationUser>>()
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<IdentityMessagesPtBrExtension>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        });

        services.AddJwtConfig(configuration);
        services.AddAuthorization();
    }
}
