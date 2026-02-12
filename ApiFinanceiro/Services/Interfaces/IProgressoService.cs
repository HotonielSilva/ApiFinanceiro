using ApiFinanceiro.Models.DTOs;

namespace ApiFinanceiro.Services.Interfaces;

public interface IProgressoService
{
    CancellationToken IniciarProcesso(string processoId, int totalItens);

    Task AtualizarProgressoAsync(string processoId, string mensagem, int itemCorrente);

    ProcessoDto? ObterStatusAtual(string processoId);

    IEnumerable<ProcessoDto> ObterTodosStatus();

    bool ProcessoFoiCancelado(string processoId);

    void CancelarProcesso(string processoId);

    Task FinalizarProcessoAsync(string processoId, bool erroOuCancelamento = false, string? mensagemFinal = null);


}
