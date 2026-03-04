using DomusPay.Domain.Entities;

namespace DomusPay.Application.Interfaces.Repositories;

public interface IPessoaRepository
{
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<Pessoa> GetByIdAsync(Guid id, bool includeTransacoes = false);
    Task CreateAsync(Pessoa pessoa);
    Task UpdateAsync(Pessoa pessoa);
    Task DeleteAsync(Guid id);
}
