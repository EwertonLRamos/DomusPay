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

    public async Task<ListagemComValoresTotaisDTO<CategoriaDTO>> GetAllAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        var categoriasComValoresTotais = categorias.Select(c => 
        {
            var totalReceitas = c.Transacoes.CalcularValorTotal(TipoTransacao.Receita);
            var totalDespesas = c.Transacoes.CalcularValorTotal(TipoTransacao.Despesa);

            return new ItemListagemComValoresTotaisDTO<CategoriaDTO>()
            {
                Item = new CategoriaDTO()
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Finalidade = c.Finalidade.ToString()
                },
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas
            };
        });

        return new ListagemComValoresTotaisDTO<CategoriaDTO>()
        {
            Itens = [.. categoriasComValoresTotais],
            TotalReceitas = categoriasComValoresTotais.Sum(c => c.TotalReceitas),
            TotalDespesas = categoriasComValoresTotais.Sum(c => c.TotalDespesas),
            SaldoTotal = categoriasComValoresTotais.Sum(c => c.Saldo)
        };
    }

    public async Task CreateAsync(CategoriaDTO categoriaDTO)
    {
        if(!Enum.TryParse<FinalidadeCategoria>(categoriaDTO.Finalidade, out var finalidade))
            throw new FinalidadeCategoriaInvalidaException(categoriaDTO.Finalidade);

        await _categoriaRepository.CreateAsync(new Categoria()
        {
            Descricao = categoriaDTO.Descricao,
            Finalidade = finalidade
        });
    }
}
