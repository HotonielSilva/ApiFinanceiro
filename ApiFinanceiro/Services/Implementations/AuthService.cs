using ApiFinanceiro.Models.Entities;
using ApiFinanceiro.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiFinanceiro.Services.Implementations;

public class AuthService(IConfiguration configuration, UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    /// <summary>
    /// Responsável por gerar o token JWT para autenticação do usuário.
    /// </summary>
    /// <param name="nomeUsuario">Nome de usuário (login) utilizado para buscar o usuário no banco.</param>
    /// <returns>Token JWT válido por 2 horas, contendo as claims e roles do usuário.</returns>
    /// <exception cref="UnauthorizedAccessException">Lançado caso o usuário não seja encontrado.</exception>
    public async Task<string> GerarTokenAsync(string nomeUsuario)
    {
        var user = await _userManager.FindByNameAsync(nomeUsuario)
            ?? throw new UnauthorizedAccessException("Usuário não encontrado.");

        var claims = await GerarClaims(user);

        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow, // Token só é válido a partir do momento atual
            Expires = DateTime.UtcNow.AddHours(2), // Expira em 2 horas
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    /// <summary>
    /// Gera as claims de identidade do usuário, incluindo ID, nome, e-mail e roles.
    /// </summary>
    /// <param name="user">Instância do usuário autenticado.</param>
    /// <returns>Lista de claims que serão embutidas no token JWT.</returns>
    private async Task<List<Claim>> GerarClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id), // ID único do usuário
            new(ClaimTypes.Name, user.UserName!),    // Nome de login
            new(ClaimTypes.Email, user.Email!)       // E-mail do usuário
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); // Roles atribuídas ao usuário

        return claims;
    }
}
