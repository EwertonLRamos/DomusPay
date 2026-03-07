using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomusPay.Infrastructure.Repositories;

public class PessoaRepository(DomusPayDbContext context) : IPessoaRepository
{
    private readonly DomusPayDbContext _context = context;

    public async Task<IEnumerable<Pessoa>> GetAllAsync()
    {
        var pessoas = await _context.Pessoas
            .Include(p => p.Transacoes)
                .ThenInclude(t => t.Categoria)
            .ToListAsync();
        
        return pessoas;
    }

    public async Task<Pessoa> GetByIdAsync(Guid id, bool includeTransacoes = false)
    {
        if (includeTransacoes)
            return await _context.Pessoas
                .Include(p => p.Transacoes)
                    .ThenInclude(t => t.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        else
            return await _context.Pessoas.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateAsync(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pessoa pessoa)
    {
        _context.Pessoas.Update(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pessoa pessoa)
    {
        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();
    }
}
