using System;

namespace DomusPay.Application.DTOs;

public class ListagemPessoaDTO
{
    public List<PessoaComValoresTotaisDTO> Pessoas { get; set; } = [];
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal SaldoTotal { get; set; }
}
