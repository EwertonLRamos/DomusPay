using System;

namespace DomusPay.Application.DTOs;

public class ItemListagemTransacaoDTO
{
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; }
    public string FinalidadeCategoria { get; set; }
    public string NomePessoa { get; set; }
}
