using DomusPay.Domain.Entities;

namespace DomusPay.Application.Interfaces.Repositories;

public interface ITransacaoRepository
{
    Task<IEnumerable<Transacao>> GetAllAsync();
    Task CreateAsync(Transacao transacao);
}
