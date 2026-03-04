namespace DomusPay.Application.DTOs;

public class ItemListagemComValoresTotaisDTO<TItem> where TItem : class
{
    public TItem Item { get; set; }
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}
