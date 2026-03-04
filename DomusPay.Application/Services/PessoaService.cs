using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;

namespace DomusPay.Application.Services;

public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<IEnumerable<PessoaComValoresTotaisDTO>> GetAllAsync()
    {
        var pessoas = await pessoaRepository.GetAllAsync();
        var pessoasComValoresTotais = pessoas.Select(p => new PessoaComValoresTotaisDTO()
        {
            Id = p.Id,
            Nome = p.Nome,
            Idade = p.Idade,
            TotalReceitas = CalcularValorTotal(p.Transacoes, FinalidadeCategoria.Receita),
            TotalDespesas = CalcularValorTotal(p.Transacoes, FinalidadeCategoria.Despesa),
            Saldo = CalcularValorTotal(p.Transacoes, FinalidadeCategoria.Receita) - 
                    CalcularValorTotal(p.Transacoes, FinalidadeCategoria.Despesa)
        });

        return pessoasComValoresTotais;
    }

    private static decimal CalcularValorTotal(IEnumerable<Transacao> transacoes, FinalidadeCategoria finalidade)
    {
        return transacoes
            .Where(t => t.Categoria?.Finalidade == finalidade)
            .Sum(t => t.Valor > 0 ? t.Valor : 0);
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
