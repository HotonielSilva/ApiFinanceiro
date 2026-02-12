using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Services.Interfaces;

namespace ApiFinanceiro.Services.Implementations;

public class AgentesService(IMegaIntegrationService megaIntegrationService) : IAgentesService
{
    private readonly IMegaIntegrationService _megaIntegrationService = megaIntegrationService;

    public async Task<Response<string>> InserirAgenteMegaAsync(MegaAgenteDto agenteDto)
    {
        try
        {
            return await _megaIntegrationService.InserirAgenteNoMegaAsync(agenteDto);
        }
        catch (Exception ex)
        {
            return new Response<string> { Success = false, Message = ex.Message };
        }
    }

    public async Task<Response<MegaAgenteDto>> ConsultarAgenteNoMegaAsync(string cnpjFilial, string codigoCliente)
    {
        try
        {
            var token = await _megaIntegrationService.GerarTokenAcessoAsync();

            if (string.IsNullOrEmpty(token))
            {
                return new Response<MegaAgenteDto>
                {
                    Success = false,
                    Message = "Não foi possível gerar o token para consulta."
                };
            }

            return await _megaIntegrationService.ConsultarAgenteNoMegaAsync(cnpjFilial, codigoCliente, token);
        }
        catch (Exception ex)
        {
            return new Response<MegaAgenteDto> { Success = false, Message = ex.Message };
        }
    }
}