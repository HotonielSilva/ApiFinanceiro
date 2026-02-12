using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceiro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Processos Mega Agentes")]
    public class AgentesController(IAgentesService agentesService) : ControllerBase
    {
        private readonly IAgentesService _agentesService = agentesService;

        [HttpPost("InserirAgenteMega")]
        public async Task<IActionResult> InserirAgenteMegaAsync([FromBody] MegaAgenteDto agenteDto)
        {
            if (agenteDto == null)
            {
                return BadRequest(new Response<string>
                {
                    Success = false,
                    Message = "Objeto de agente inválido."
                });
            }

            try
            {
                var response = await _agentesService.InserirAgenteMegaAsync(agenteDto);

                if (response.Success)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = $"Erro crítico na integração: {ex.Message}"
                });
            }
        }


        [HttpGet("ConsultarAgenteMega/{cnpjFilial}/{codigoCliente}")]
        public async Task<IActionResult> ConsultarAgenteMegaAsync(string cnpjFilial, string codigoCliente)
        {
            if (string.IsNullOrEmpty(cnpjFilial) || string.IsNullOrEmpty(codigoCliente))
                return BadRequest(new Response<string> { Success = false, Message = "Parâmetros de consulta inválidos." });

            try
            {
                var response = await _agentesService.ConsultarAgenteNoMegaAsync(cnpjFilial, codigoCliente);

                if (response.Success)
                    return Ok(response);

                return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>
                {
                    Success = false,
                    Message = $"Erro ao consultar agente: {ex.Message}"
                });
            }
        }
    }
}