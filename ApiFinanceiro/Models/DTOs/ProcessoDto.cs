namespace ApiFinanceiro.Models.DTOs;

public class ProcessoDto
{
    public string ProcessoId { get; set; } = string.Empty;
    public string Etapa { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public int ItemCorrente { get; set; }
    public int TotalItens { get; set; }
    public bool EstaRodando { get; set; }
    public bool TeveErroOuCancelamento { get; set; }
    public double Percentual => TotalItens > 0 ? Math.Round((double)ItemCorrente / TotalItens * 100, 2) : 0;
}
