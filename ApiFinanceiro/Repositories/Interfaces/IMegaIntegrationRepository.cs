using ApiFinanceiro.Models.DTOs;

namespace ApiFinanceiro.Repositories.Interfaces;

public interface IMegaIntegrationRepository
{
    /// <summary>
    /// Gerar token de acesso para autenticação na API do Mega.
    /// </summary>
    /// <returns></returns>
    Task<string?> GerarTokenAcessoAsync();

    /// <summary>
    /// Inserir novo agente no mega.
    /// </summary>
    /// <param name="agenteDto"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    Task<Response<string>> InserirAgenteNoMegaAsync(MegaAgenteDto agenteDto, string accessToken);

    /// <summary>
    /// Consultar agente no mega utilizando o cnpj da filial e o código do cliente.
    /// </summary>
    /// <param name="cnpjFilial"></param>
    /// <param name="codigoCliente"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    Task<Response<MegaAgenteDto>> ConsultarAgenteNoMegaAsync(string cnpjFilial, string codigoCliente, string accessToken);
}