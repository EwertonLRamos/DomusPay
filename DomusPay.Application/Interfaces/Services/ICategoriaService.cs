using DomusPay.Application.DTOs;

namespace DomusPay.Application.Interfaces.Services;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDTO>> GetAllAsync();
    Task CreateAsync(CategoriaDTO categoriaDTO);
}
