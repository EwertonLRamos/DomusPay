using DomusPay.Application.DTOs;
using DomusPay.Application.Extensions;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;

namespace DomusPay.Application.Services;

public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<ListagemComValoresTotaisDTO<PessoaDTO>> GetAllAsync()
    {
        var pessoas = await pessoaRepository.GetAllAsync();
        var pessoasComValoresTotais = pessoas.Select(p => 
        {
            var totalReceitas = p.Transacoes.CalcularValorTotal(TipoTransacao.Receita);
            var totalDespesas = p.Transacoes.CalcularValorTotal(TipoTransacao.Despesa);

            return new ItemListagemComValoresTotaisDTO<PessoaDTO>()
            {
                Item = new PessoaDTO()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Idade = p.Idade
                },
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas
            };
        });

        return new ListagemComValoresTotaisDTO<PessoaDTO>()
        {
            Itens = [.. pessoasComValoresTotais],
            TotalReceitas = pessoasComValoresTotais.Sum(p => p.TotalReceitas),
            TotalDespesas = pessoasComValoresTotais.Sum(p => p.TotalDespesas),
            SaldoTotal = pessoasComValoresTotais.Sum(p => p.Saldo)
        };
    }

    public async Task<PessoaDTO> GetByIdAsync(Guid id)
    {
        var pessoa = await pessoaRepository.GetByIdAsync(id, includeTransacoes: true) ?? 
            throw new FileNotFoundException("Pessoa não encontrada.");
        
        return new PessoaDTO() 
        { 
            Id = pessoa.Id, 
            Nome = pessoa.Nome, 
            Idade = pessoa.Idade 
        };
    }

    public async Task CreateAsync(PessoaDTO pessoaDTO)
    {
        await pessoaRepository.CreateAsync(new Pessoa() 
        {  
            Nome = pessoaDTO.Nome, 
            Idade = pessoaDTO.Idade 
        });
    }

    public async Task UpdateAsync(Guid id, PessoaDTO pessoaDTO)
    {
        if(id != pessoaDTO.Id)
            throw new ArgumentException("O ID fornecido não corresponde ao ID da pessoa a ser atualizada.");

        var pessoa = await pessoaRepository.GetByIdAsync(id) ?? throw new FileNotFoundException("Pessoa não encontrada.");

        pessoa.Nome = pessoaDTO.Nome;
        pessoa.Idade = pessoaDTO.Idade;

        await pessoaRepository.UpdateAsync(pessoa);
    }

    public async Task DeleteAsync(Guid id)
    {
        await pessoaRepository.DeleteAsync(id);
    }
}
