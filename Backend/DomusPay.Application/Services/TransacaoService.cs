using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;
using DomusPay.Domain.Exceptions;

namespace DomusPay.Application.Services;

public class TransacaoService(
    ITransacaoRepository transacaoRepository, 
    IPessoaRepository pessoaRepository, 
    ICategoriaRepository categoriaRepository)
     : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository = transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task CreateAsync(CadastroTransacaoDTO transacaoDTO)
    {
        var pessoa = await _pessoaRepository.GetByIdAsync(transacaoDTO.PessoaId) ?? 
            throw new PessoaNaoEncontradaOuNaoExisteException(transacaoDTO.PessoaId);
        
        var categoria = await _categoriaRepository.GetByIdAsync(transacaoDTO.CategoriaId) ?? 
            throw new CategoriaNaoEncontradaOuNaoExisteException(transacaoDTO.CategoriaId);

        if(!Enum.TryParse<TipoTransacao>(transacaoDTO.Tipo, out var tipoTransacao))
            throw new TipoTransferenciaInvalidoException(transacaoDTO.Tipo);

        if(categoria.Finalidade != FinalidadeCategoria.Ambas && categoria.Finalidade.ToString() != tipoTransacao.ToString())
            throw new CategoriaIncompativelComFinalidadeException();

        if(pessoa.Idade < 18 && tipoTransacao == TipoTransacao.Receita)
            throw new PessoaMenorNaoPodeTerReceitaException();

        await _transacaoRepository.CreateAsync(new Transacao()
        {   
            Descricao = transacaoDTO.Descricao,
            Valor = transacaoDTO.Valor,
            Tipo = tipoTransacao,
            PessoaId = transacaoDTO.PessoaId,
            CategoriaId = transacaoDTO.CategoriaId
        });
    }

    public async Task<ListagemBaseDTO<ItemListagemTransacaoDTO>> GetAllAsync()
    {
        var transacoes = await _transacaoRepository.GetAllAsync();

        return new ListagemBaseDTO<ItemListagemTransacaoDTO>()
        {
            Itens = [.. transacoes.Select(t => new ItemListagemTransacaoDTO()
            {
                Descricao = t.Descricao,
                Valor = t.Valor,
                Tipo = t.Tipo.ToString(),
                NomePessoa = t.Pessoa.Nome,
                FinalidadeCategoria = t.Categoria.Descricao
            })]
        };
    }
}
