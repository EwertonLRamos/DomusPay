using DomusPay.Application.DTOs;

namespace DomusPay.Application.Interfaces.Services;

public interface ICategoriaService
{
    Task<ListagemComValoresTotaisDTO<CategoriaDTO>> GetAllAsync();
    Task CreateAsync(CategoriaDTO categoriaDTO);
}
