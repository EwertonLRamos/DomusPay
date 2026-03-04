namespace DomusPay.Application.DTOs;

public class PessoaComValoresTotaisDTO : PessoaDTO
{
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}
