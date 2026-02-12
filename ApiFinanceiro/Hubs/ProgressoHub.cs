using ApiFinanceiro.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ApiFinanceiro.Hubs;

[Authorize]
public class ProgressoHub(IProgressoService progressoService) : Hub
{
    private readonly IProgressoService _progressoService = progressoService;

    public async Task MonitorarProcesso(string processoId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, processoId);
    }

    public async Task EntrarMonitoramentoGlobal()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "MonitoramentoGlobal");
    }

    public void CancelarProcesso(string processoId)
    {
        _progressoService.CancelarProcesso(processoId);
    }
}
