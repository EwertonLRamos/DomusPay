using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomusPay.Infrastructure.Repositories;

public class CategoriaRepository(DomusPayDbContext context) : ICategoriaRepository
{
    private readonly DomusPayDbContext _context = context;

    public async Task CreateAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task<Categoria> GetByIdAsync(Guid id)
    {
        return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _context.Categorias
            .Include(c => c.Transacoes)
            .ToAsyncEnumerable()
            .ToListAsync();
    }
}
