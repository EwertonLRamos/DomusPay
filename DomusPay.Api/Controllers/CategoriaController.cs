using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DomusPay.Api.Controllers
{
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController(ICategoriaService categoriaService) : ControllerBase
    {
        private readonly ICategoriaService _categoriaService = categoriaService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ListagemComValoresTotaisDTO<ItemListagemCategoriaDTO>>> GetAll()
        {
            var categorias = await _categoriaService.GetAllAsync();

            if (categorias is null || categorias.Itens.Count == 0)
                return NotFound();

            return Ok(categorias);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Create(CadastroCategoriaDTO cadastroCategoria)
        {
            await _categoriaService.CreateAsync(cadastroCategoria);
            return Created();
        }
    }
}
