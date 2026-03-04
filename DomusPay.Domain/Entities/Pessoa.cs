namespace DomusPay.Domain.Entities;

public class Pessoa : BaseEntity
{
    public string Nome { get; set; }
    public int Idade { get; set; }

    public ICollection<Transacao> Transacoes { get; set; }
}
