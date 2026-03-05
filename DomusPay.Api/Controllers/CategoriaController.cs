using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DomusPay.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar as operações relacionadas às categorias, como criação e consulta de categorias.
    /// </summary>
    /// <param name="categoriaService"></param>
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController(ICategoriaService categoriaService) : ControllerBase
    {
        private readonly ICategoriaService _categoriaService = categoriaService;


        /// <summary>
        /// Retrieves a list of all categories along with their total values.
        /// </summary>
        /// <returns>
        /// HTTP 200 OK with the list of categories if found.
        /// HTTP 404 Not Found if no categories are found.
        /// HTTP 400 Bad Request if the request is invalid.
        /// </returns>
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


        /// <summary>
        /// Creates a new category with the provided data.
        /// </summary>
        /// <param name="cadastroCategoria"></param>
        /// <returns>
        /// HTTP 201 Created if the creation is successful.
        /// HTTP 400 Bad Request if the provided data is invalid.
        /// </returns>
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
