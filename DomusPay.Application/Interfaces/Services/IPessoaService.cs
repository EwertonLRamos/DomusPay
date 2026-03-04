using DomusPay.Application.DTOs;

namespace DomusPay.Application.Interfaces.Services;

public interface IPessoaService
{
    Task<ListagemComValoresTotaisDTO<PessoaDTO>> GetAllAsync();
    Task<PessoaDTO> GetByIdAsync(Guid id);
    Task CreateAsync(PessoaDTO pessoaDTO);
    Task UpdateAsync(Guid id, PessoaDTO pessoaDTO);
    Task DeleteAsync(Guid id);
}
