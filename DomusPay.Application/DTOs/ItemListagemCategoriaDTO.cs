namespace DomusPay.Application.DTOs;

public class ItemListagemCategoriaDTO
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public string Finalidade { get; set; }
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}
