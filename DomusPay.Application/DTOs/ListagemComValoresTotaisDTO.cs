using System;

namespace DomusPay.Application.DTOs;

public class ListagemComValoresTotaisDTO<TItem> where TItem : class
{
    public List<ItemListagemComValoresTotaisDTO<TItem>> Itens { get; set; } = [];
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal SaldoTotal { get; set; }
}
