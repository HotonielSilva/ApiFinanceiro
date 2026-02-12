namespace ApiFinanceiro.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Criar token do usuário
    /// </summary>
    /// <param name="nomeUsuario"></param>
    /// <returns>token gerado ou erro</returns>
    Task<string> GerarTokenAsync(string nomeUsuario);

}
