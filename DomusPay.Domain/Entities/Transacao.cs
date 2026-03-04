using DomusPay.Domain.Enums;

namespace DomusPay.Domain.Entities;

public class Transacao : BaseEntity
{
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public Guid CategoriaId { get; set; }
    public Guid PessoaId { get; set; }

    public Categoria Categoria { get; set; }
    public Pessoa Pessoa { get; set; }
}
