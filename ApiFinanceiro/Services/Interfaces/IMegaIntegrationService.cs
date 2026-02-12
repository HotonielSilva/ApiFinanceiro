using ApiFinanceiro.Models.DTOs;

namespace ApiFinanceiro.Services.Interfaces
{
    public interface IMegaIntegrationService
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
        Task<Response<string>> InserirAgenteNoMegaAsync(MegaAgenteDto agenteDto, string? accessToken = null);

        /// <summary>
        /// consulta Agente no mega por cnpj da filial e codigo do cliente, caso exista retorna o id do agente, caso contrário retorna null.
        /// </summary>
        /// <param name="cnpjFilial"></param>
        /// <param name="codigoCliente"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<Response<MegaAgenteDto>> ConsultarAgenteNoMegaAsync(string cnpjFilial, string codigoCliente, string? accessToken = null);
    }
}