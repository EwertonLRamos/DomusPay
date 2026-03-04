using DomusPay.Application.DTOs;

namespace DomusPay.Application.Interfaces.Services;

public interface ITransacaoService
{
    Task<ListagemBaseDTO<ItemListagemTransacaoDTO>> GetAllAsync();
    Task CreateAsync(CadastroTransacaoDTO transacaoDTO);
}
