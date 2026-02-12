using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Repositories.Interfaces;
using ApiFinanceiro.Services.Interfaces;

namespace ApiFinanceiro.Services.Implementations;

public class MegaIntegrationService(IMegaIntegrationRepository megaRepo) : IMegaIntegrationService
{
    private readonly IMegaIntegrationRepository _megaRepo = megaRepo;

    public async Task<string?> GerarTokenAcessoAsync() => await _megaRepo.GerarTokenAcessoAsync();

    public async Task<Response<string>> InserirAgenteNoMegaAsync(MegaAgenteDto agenteDto, string? accessToken = null)
    {
        var token = accessToken ?? await GerarTokenAcessoAsync();
        if (string.IsNullOrEmpty(token))
            return new Response<string> { Success = false, Message = "Token inválido." };

        return await _megaRepo.InserirAgenteNoMegaAsync(agenteDto, token);
    }

    // CORREÇÃO: Tipagem correta (MegaAgenteDto) e parâmetros de negócio (CNPJ/Código)
    public async Task<Response<MegaAgenteDto>> ConsultarAgenteNoMegaAsync(string cnpjFilial, string codigoCliente, string? accessToken = null)
    {
        var token = accessToken ?? await GerarTokenAcessoAsync();

        if (string.IsNullOrEmpty(token))
            return new Response<MegaAgenteDto> { Success = false, Message = "Token inválido." };

        return await _megaRepo.ConsultarAgenteNoMegaAsync(cnpjFilial, codigoCliente, token);
    }
}