using ApiFinanceiro.Hubs;
using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ApiFinanceiro.Services.Implementations;

public class ProgressoService(IHubContext<ProgressoHub> hubContext) : IProgressoService
{
    private readonly IHubContext<ProgressoHub> _hubContext = hubContext;
    private static readonly ConcurrentDictionary<string, ProcessoDto> _estados = new();
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _canceladores = new();

    public CancellationToken IniciarProcesso(string processoId, int totalItens)
    {
        var cts = new CancellationTokenSource();
        _canceladores[processoId] = cts;

        _estados[processoId] = new ProcessoDto
        {
            ProcessoId = processoId,
            TotalItens = totalItens,
            EstaRodando = true,
            Mensagem = "Iniciando..."
        };

        return cts.Token;
    }

    public async Task AtualizarProgressoAsync(string processoId, string mensagem, int itemCorrente)
    {
        if (_estados.TryGetValue(processoId, out var status))
        {
            status.Mensagem = mensagem;
            status.ItemCorrente = itemCorrente;

            // Envia para quem está ouvindo o processo específico
            await _hubContext.Clients.Group(processoId).SendAsync("ReceberProgresso", status);

            // Envia para a tela de monitoramento global
            await _hubContext.Clients.Group("MonitoramentoGlobal").SendAsync("AtualizarListaProcessos", status);
        }
    }

    public ProcessoDto? ObterStatusAtual(string processoId) =>
        _estados.TryGetValue(processoId, out var status) ? status : null;

    public IEnumerable<ProcessoDto> ObterTodosStatus()
    {
        return _estados.Values;
    }

    public bool ProcessoFoiCancelado(string processoId)
    {
        if (_canceladores.TryGetValue(processoId, out var cts))
        {
            return cts.IsCancellationRequested;
        }
        return false;
    }

    public void CancelarProcesso(string processoId) =>
    _canceladores.GetValueOrDefault(processoId)?.Cancel();

    public async Task FinalizarProcessoAsync(string processoId, bool erroOuCancelamento = false, string? mensagemFinal = null)
    {
        if (_estados.TryGetValue(processoId, out var status))
        {
            status.EstaRodando = false;
            status.TeveErroOuCancelamento = erroOuCancelamento;

            if (!string.IsNullOrEmpty(mensagemFinal))
                status.Mensagem = mensagemFinal;

            await _hubContext.Clients.Group(processoId).SendAsync("ReceberProgresso", status);
        }

        _canceladores.TryRemove(processoId, out _);
    }

}
