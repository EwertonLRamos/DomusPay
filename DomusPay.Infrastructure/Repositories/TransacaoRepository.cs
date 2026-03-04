using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomusPay.Infrastructure.Repositories;

public class TransacaoRepository(DomusPayDbContext context) : ITransacaoRepository
{
    private readonly DomusPayDbContext _context = context;

    public async Task CreateAsync(Transacao transacao)
    {
        await _context.Transacoes.AddAsync(transacao);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transacao>> GetAllAsync()
    {
        return await _context.Transacoes
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .ToListAsync();
    }
}
