using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressoController(IProgressoService progressoService) : ControllerBase
{
    private readonly IProgressoService _progressoService = progressoService;

    [HttpGet("ConsultarStatus/{processoId}")]
    [ProducesResponseType(typeof(ProcessoDto), StatusCodes.Status200OK)]
    public IActionResult ConsultarStatus(string processoId)
    {
        var status = _progressoService.ObterStatusAtual(processoId);

        if (status == null)
            return Ok(new { EstaRodando = false, Mensagem = "Nenhum processo encontrado." });
        return Ok(status);
    }

    [HttpGet("ConsultarTodos")]
    [ProducesResponseType(typeof(IEnumerable<ProcessoDto>), StatusCodes.Status200OK)]
    public IActionResult ConsultarTodos()
    {
        var todos = _progressoService.ObterTodosStatus();
        return Ok(todos);
    }
}