using DomusPay.Application.DTOs;

namespace DomusPay.Application.Interfaces.Services;

public interface IPessoaService
{
    Task<ListagemComValoresTotaisDTO<ItemListagemPessoaDTO>> GetAllAsync();
    Task<PessoaDTO> GetByIdAsync(Guid id);
    Task CreateAsync(CadastroPessoaDTO cadastroPessoa);
    Task UpdateAsync(Guid id, PessoaDTO pessoaDTO);
    Task DeleteAsync(Guid id);
}
