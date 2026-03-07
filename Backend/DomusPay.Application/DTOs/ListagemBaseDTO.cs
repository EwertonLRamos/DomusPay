namespace DomusPay.Application.DTOs;

public class ListagemBaseDTO<TItem> where TItem : class
{
    public List<TItem> Itens { get; set; } = [];
}
