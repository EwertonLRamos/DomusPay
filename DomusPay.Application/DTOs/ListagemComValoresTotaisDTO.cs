namespace DomusPay.Application.DTOs;

public class ListagemComValoresTotaisDTO<TItem> : ListagemBaseDTO<TItem> where TItem : class
{
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal SaldoTotal { get; set; }
}
