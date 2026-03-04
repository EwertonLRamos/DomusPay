using DomusPay.Application.DTOs;

namespace DomusPay.Application.Interfaces.Services;

public interface ITransacaoService
{
    Task<IEnumerable<ItemListagemTransacaoDTO>> GetAllAsync();
    Task CreateAsync(CadastroTransacaoDTO transacaoDTO);
}
