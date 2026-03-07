using DomusPay.Domain.Enums;

namespace DomusPay.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Descricao { get; set; }
    public FinalidadeCategoria Finalidade { get; set; }

    public ICollection<Transacao> Transacoes { get; set; }
}
