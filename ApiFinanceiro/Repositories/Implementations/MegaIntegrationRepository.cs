using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Repositories.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiFinanceiro.Repositories.Implementations;

public class MegaIntegrationRepository(HttpClient httpClient, IConfiguration config) : IMegaIntegrationRepository
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _config = config;

    /// <summary>
    /// Gerar token de acesso para autenticação na API do Mega.
    /// </summary>
    /// <returns></returns>
    public async Task<string?> GerarTokenAcessoAsync()
    {
        var baseUrl = _config["MegaIntegration:BaseUrl"];

        if (string.IsNullOrEmpty(baseUrl))
        {
            throw new InvalidOperationException("A URL Base do Mega não foi configurada no appsettings.json.");
        }

        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        var login = new
        {
            userName = _config["MegaIntegration:UserName"],
            password = _config["MegaIntegration:Password"]
        };

        var response = await _httpClient.PostAsJsonAsync("api/Auth/SignIn", login);

        if (!response.IsSuccessStatusCode) return null;

        var tokenDto = await response.Content.ReadFromJsonAsync<TokenMegaDto>();
        return tokenDto?.AccessToken;
    }

    /// <summary>
    /// Inserir novo agente no mega.
    /// </summary>
    /// <param name="agenteDto"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public async Task<Response<string>> InserirAgenteNoMegaAsync(MegaAgenteDto agenteDto, string accessToken)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Não envia campos nulos
                WriteIndented = true
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Add("FIL_IN_CODIGO", "900");

            var json = JsonSerializer.Serialize(agenteDto, options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "api/globalagente/AgenteCliente",
                content
            );

            //var response = await _httpClient.PostAsJsonAsync("api/globalagente/AgenteCliente", agenteDto, options);
            var conteudo = await response.Content.ReadAsStringAsync();

            return new Response<string>
            {
                Success = response.IsSuccessStatusCode,
                Data = conteudo,
                Message = response.IsSuccessStatusCode ? "Agente inserido com sucesso." : "Erro ao inserir agente no Mega."
            };
        }
        catch (Exception ex)
        {
            return new Response<string> { Success = false, Message = $"Falha na comunicação: {ex.Message}" };
        }
    }

    /// <summary>
    /// Consultar agente no mega utilizando o cnpj da filial e o código do cliente.
    /// </summary>
    /// <param name="cnpjFilial"></param>
    /// <param name="codigoCliente"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public async Task<Response<MegaAgenteDto>> ConsultarAgenteNoMegaAsync(string cnpjFilial, string codigoCliente, string accessToken)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"api/globalagente/AgenteCliente/codigocliente/{cnpjFilial}/{codigoCliente}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var agente = await response.Content.ReadFromJsonAsync<MegaAgenteDto>();
                return new Response<MegaAgenteDto> { Success = true, Data = agente };
            }

            return new Response<MegaAgenteDto> { Success = false, Message = "Agente não encontrado no Mega." };
        }
        catch (Exception ex) { return new Response<MegaAgenteDto> { Success = false, Message = ex.Message }; }
    }
}