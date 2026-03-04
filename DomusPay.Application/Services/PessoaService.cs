using DomusPay.Application.DTOs;
using DomusPay.Application.Extensions;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;
using DomusPay.Domain.Exceptions;

namespace DomusPay.Application.Services;

public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<ListagemComValoresTotaisDTO<ItemListagemPessoaDTO>> GetAllAsync()
    {
        var pessoas = await pessoaRepository.GetAllAsync();
        var pessoasComValoresTotais = pessoas.Select(p => 
        {
            var totalReceitas = p.Transacoes is null ? 0 : p.Transacoes.CalcularValorTotal(TipoTransacao.Receita);
            var totalDespesas = p.Transacoes is null ? 0 : p.Transacoes.CalcularValorTotal(TipoTransacao.Despesa);

            return new ItemListagemPessoaDTO()
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas
            };
        });

        return new ListagemComValoresTotaisDTO<ItemListagemPessoaDTO>()
        {
            Itens = [.. pessoasComValoresTotais],
            TotalReceitas = pessoasComValoresTotais.Sum(p => p.TotalReceitas),
            TotalDespesas = pessoasComValoresTotais.Sum(p => p.TotalDespesas),
            SaldoTotal = pessoasComValoresTotais.Sum(p => p.Saldo)
        };
    }

    public async Task<PessoaDTO> GetByIdAsync(Guid id)
    {
        var pessoa = await pessoaRepository.GetByIdAsync(id) ?? 
            throw new PessoaNaoEncontradaOuNaoExisteException(id);
        
        return new PessoaDTO() 
        { 
            Id = pessoa.Id, 
            Nome = pessoa.Nome, 
            Idade = pessoa.Idade 
        };
    }

    public async Task CreateAsync(CadastroPessoaDTO cadastroPessoa)
    {
        await pessoaRepository.CreateAsync(new Pessoa() 
        {  
            Nome = cadastroPessoa.Nome, 
            Idade = cadastroPessoa.Idade 
        });
    }

    public async Task UpdateAsync(Guid id, PessoaDTO pessoaDTO)
    {
        if(id != pessoaDTO.Id)
            throw new ArgumentException("O ID fornecido não corresponde ao ID da pessoa a ser atualizada.");

        var pessoa = await pessoaRepository.GetByIdAsync(id) ?? 
            throw new PessoaNaoEncontradaOuNaoExisteException(id);

        pessoa.Nome = pessoaDTO.Nome;
        pessoa.Idade = pessoaDTO.Idade;

        await pessoaRepository.UpdateAsync(pessoa);
    }

    public async Task DeleteAsync(Guid id)
    {
        var pessoa = await pessoaRepository.GetByIdAsync(id) ?? 
            throw new PessoaNaoEncontradaOuNaoExisteException(id);

        await pessoaRepository.DeleteAsync(pessoa);
    }
}
