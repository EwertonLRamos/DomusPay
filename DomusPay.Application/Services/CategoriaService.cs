using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Repositories;
using DomusPay.Application.Interfaces.Services;
using DomusPay.Domain.Entities;
using DomusPay.Domain.Enums;

namespace DomusPay.Application.Services;

public class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<IEnumerable<CategoriaDTO>> GetAllAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return categorias.Select(c => new CategoriaDTO()
        {
            Descricao = c.Descricao,
            Finalidade = c.Finalidade.ToString()
        });
    }

    public async Task CreateAsync(CategoriaDTO categoriaDTO)
    {
        await _categoriaRepository.CreateAsync(new Categoria()
        {
            Descricao = categoriaDTO.Descricao,
            Finalidade = Enum.Parse<FinalidadeCategoria>(categoriaDTO.Finalidade)
        });
    }
}
