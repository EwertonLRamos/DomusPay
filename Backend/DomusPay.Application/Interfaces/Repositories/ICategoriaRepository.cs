using DomusPay.Domain.Entities;

namespace DomusPay.Application.Interfaces.Repositories;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria> GetByIdAsync(Guid id);
    Task CreateAsync(Categoria categoria);
}
