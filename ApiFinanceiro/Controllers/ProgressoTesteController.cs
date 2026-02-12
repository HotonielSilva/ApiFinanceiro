using ApiFinanceiro.Models.DTOs;
using ApiFinanceiro.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressoTesteController(IProgressoService progressoService) : ControllerBase
{
    private readonly IProgressoService _progressoService = progressoService;

    [HttpPost("SimularProcesso/{processoId}/{total}")]
    public IActionResult Simular(string processoId, int total)
    {
        // "Fire and Forget": Dispara a tarefa em segundo plano e libera o request
        _ = Task.Run(async () =>
        {
            try
            {
                // 1. Notifica o início do processo no Service e Dicionário
                _progressoService.IniciarProcesso(processoId, total);

                for (int i = 1; i <= total; i++)
                {
                    // Simula uma carga de trabalho
                    await Task.Delay(200);

                    // 2. Verifica se o usuário clicou no botão "Cancelar" no HTML
                    if (_progressoService.ProcessoFoiCancelado(processoId))
                    {
                        // Finaliza como ERRO/CANCELADO para pintar a barra de VERMELHO
                        await _progressoService.FinalizarProcessoAsync(
                            processoId,
                            erroOuCancelamento: true,
                            mensagemFinal: "Interrompido pelo usuário"
                        );
                        return; // Aborta a execução da Task
                    }

                    // 3. Atualiza o progresso (Envia sinal is-primary / Azul)
                    await _progressoService.AtualizarProgressoAsync(
                        processoId,
                        $"Processando item {i} de {total}...",
                        i
                    );
                }

                // 4. Finaliza com SUCESSO (Envia sinal is-success / Verde)
                await _progressoService.FinalizarProcessoAsync(
                    processoId,
                    erroOuCancelamento: false,
                    mensagemFinal: "Processo Finalizado com Sucesso!"
                );
            }
            catch (Exception ex)
            {
                // 5. Finaliza com ERRO CRÍTICO (Envia sinal is-danger / Vermelho)
                // Se cair aqui, a barra no seu HTML ficará vermelha
                await _progressoService.FinalizarProcessoAsync(
                    processoId,
                    erroOuCancelamento: true,
                    mensagemFinal: $"Erro na simulação: {ex.Message}"
                );
            }
        });

        // Retorna 202 (Accepted) para o cliente saber que a tarefa começou
        return Accepted(new Response<object>
        {
            Success = true,
            Message = $"Simulação do processo '{processoId}' iniciada com sucesso."
        });
    }
}