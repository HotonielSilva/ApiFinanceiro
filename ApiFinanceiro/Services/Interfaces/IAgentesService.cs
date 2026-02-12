using ApiFinanceiro.Models.DTOs;

namespace ApiFinanceiro.Services.Interfaces;

public interface IAgentesService
{
    /// <summary>
    /// Inserir novo agente no Mega.
    /// </summary>
    /// <param name="agenteDto"></param>
    /// <returns></returns>
    Task<Response<string>> InserirAgenteMegaAsync(MegaAgenteDto agenteDto);

    /// <summary>
    /// Consultar agente no Mega pelo CNPJ da filial e código do cliente.
    /// </summary>
    /// <param name="cnpjFilial"></param>
    /// <param name="codigoCliente"></param>
    /// <returns></returns>
    Task<Response<MegaAgenteDto>> ConsultarAgenteNoMegaAsync(string cnpjFilial, string codigoCliente);
}