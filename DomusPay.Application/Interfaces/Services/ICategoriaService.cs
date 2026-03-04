using DomusPay.Application.DTOs;

namespace DomusPay.Application.Interfaces.Services;

public interface ICategoriaService
{
    Task<ListagemComValoresTotaisDTO<ItemListagemCategoriaDTO>> GetAllAsync();
    Task CreateAsync(CadastroCategoriaDTO cadastroCategoria);
}
