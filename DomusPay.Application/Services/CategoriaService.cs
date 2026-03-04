using DomusPay.Application.DTOs;
using DomusPay.Application.Extensions;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;
using DomusPay.Domain.Exceptions;

namespace DomusPay.Application.Services;

public class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<ListagemComValoresTotaisDTO<ItemListagemCategoriaDTO>> GetAllAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        var categoriasComValoresTotais = categorias.Select(c => 
        {
            var totalReceitas = c.Transacoes is null ? 0 : c.Transacoes.CalcularValorTotal(TipoTransacao.Receita);
            var totalDespesas = c.Transacoes is null ? 0 : c.Transacoes.CalcularValorTotal(TipoTransacao.Despesa);

            return new ItemListagemCategoriaDTO()
            {
                Id = c.Id,
                Descricao = c.Descricao,
                Finalidade = c.Finalidade.ToString(),
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas
            };
        });

        return new ListagemComValoresTotaisDTO<ItemListagemCategoriaDTO>()
        {
            Itens = [.. categoriasComValoresTotais],
            TotalReceitas = categoriasComValoresTotais.Sum(c => c.TotalReceitas),
            TotalDespesas = categoriasComValoresTotais.Sum(c => c.TotalDespesas),
            SaldoTotal = categoriasComValoresTotais.Sum(c => c.Saldo)
        };
    }

    public async Task CreateAsync(CadastroCategoriaDTO cadastroCategoria)
    {
        if(!Enum.TryParse<FinalidadeCategoria>(cadastroCategoria.Finalidade, out var finalidade))
            throw new FinalidadeCategoriaInvalidaException(cadastroCategoria.Finalidade);

        await _categoriaRepository.CreateAsync(new Categoria()
        {
            Descricao = cadastroCategoria.Descricao,
            Finalidade = finalidade
        });
    }
}
