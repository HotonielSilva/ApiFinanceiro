using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiFinanceiro.Configurations.Jwt;

public static class JwtConfigExtension
{
    /// <summary>
    /// Configura a autenticação JWT no pipeline de serviços da aplicação.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação (DI container).</param>
    /// <param name="configuration">Fonte de configuração contendo as chaves JWT.</param>
    /// <remarks>
    /// Esta configuração define os parâmetros de validação do token, como emissor, audiência, chave de assinatura,
    /// e também limpa os mapeamentos automáticos de claims para evitar conflitos.
    /// </remarks>
    public static void AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

        // 🔥 ESSENCIAL: limpa mapeamentos automáticos de claims para evitar substituições indesejadas
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(options =>
        {
            // Define o esquema padrão de autenticação como JWT Bearer
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true; // Só permite uso em ambientes HTTPS
            options.SaveToken = true; // Salva o token no contexto de autenticação

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // Valida o emissor do token
                ValidateAudience = true, // Valida a audiência do token
                ValidateLifetime = true, // Valida se o token está expirado
                ValidateIssuerSigningKey = true, // Valida a chave de assinatura

                ValidIssuer = jwtSection["Issuer"], // Emissor esperado
                ValidAudience = jwtSection["Audience"], // Audiência esperada
                IssuerSigningKey = new SymmetricSecurityKey(key), // Chave secreta usada para assinar o token

                ClockSkew = TimeSpan.FromMinutes(5), // Tolerância de tempo para expiração
                RoleClaimType = ClaimTypes.Role, // Define o tipo de claim para roles
                NameClaimType = ClaimTypes.Name // Define o tipo de claim para nome
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Tenta ler o token da query string enviada pelo SignalR
                    var accessToken = context.Request.Query["access_token"];

                    // Se houver um token na Query e a rota NÃO for vazia
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
    }
}
