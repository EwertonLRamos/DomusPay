namespace DomusPay.Application.DTOs;

public class CadastroTransacaoDTO
{
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; }
    public Guid CategoriaId { get; set; }
    public Guid PessoaId { get; set; }
}
