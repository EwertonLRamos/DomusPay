using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;

namespace DomusPay.Application.Services;

public class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<ListagemComValoresTotaisDTO<CategoriaDTO>> GetAllAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        var categoriasComValoresTotais = categorias.Select(c => new ItemListagemComValoresTotaisDTO<CategoriaDTO>()
        {
            Item = new CategoriaDTO()
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Finalidade = c.Finalidade.ToString()
            },
            TotalReceitas = CalcularValorTotal(c.Transacoes, TipoTransacao.Receita),
            TotalDespesas = CalcularValorTotal(c.Transacoes, TipoTransacao.Despesa),
            Saldo = CalcularValorTotal(c.Transacoes, TipoTransacao.Receita) - 
                    CalcularValorTotal(c.Transacoes, TipoTransacao.Despesa)
        });

        return new ListagemComValoresTotaisDTO<CategoriaDTO>()
        {
            Itens = [.. categoriasComValoresTotais],
            TotalReceitas = categoriasComValoresTotais.Sum(c => c.TotalReceitas),
            TotalDespesas = categoriasComValoresTotais.Sum(c => c.TotalDespesas),
            SaldoTotal = categoriasComValoresTotais.Sum(c => c.Saldo)
        };
    }

    private static decimal CalcularValorTotal(IEnumerable<Transacao> transacoes, TipoTransacao tipoTransacao)
    {
        return transacoes
            .Where(t => t.Tipo == tipoTransacao)
            .Sum(t => t.Valor > 0 ? t.Valor : 0);
    }

    public async Task CreateAsync(CategoriaDTO categoriaDTO)
    {
        await _categoriaRepository.CreateAsync(new Categoria()
        {
            Descricao = categoriaDTO.Descricao,
            Finalidade = Enum.Parse<FinalidadeCategoria>(categoriaDTO.Finalidade)
        });
    }
}
