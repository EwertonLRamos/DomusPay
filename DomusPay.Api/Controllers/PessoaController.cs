using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DomusPay.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar as operações relacionadas às pessoas, como criação, atualização, exclusão e consulta de pessoas.
    /// </summary>
    /// <param name="pessoaService"></param>
    [Route("api/pessoa")]
    [ApiController]
    public class PessoaController(
        IPessoaService pessoaService
    ) : ControllerBase
    {
        private readonly IPessoaService _pessoaService = pessoaService;

        /// <summary>
        /// Retrieves a list of all people along with their total values.
        /// </summary>
        /// <returns>
        /// HTTP 200 OK with the list of people if found.
        /// HTTP 404 Not Found if no people are found.
        /// HTTP 400 Bad Request if the request is invalid.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ListagemComValoresTotaisDTO<ItemListagemPessoaDTO>>> GetAll()
        {
            var pessoas = await _pessoaService.GetAllAsync();

            if (pessoas is null || pessoas.Itens.Count == 0)
                return NotFound();

            return Ok(pessoas);
        }

        /// <summary>
        /// Retrieves a person by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// HTTP 200 OK with the person data if found.
        /// HTTP 404 Not Found if the person with the specified ID does not exist.
        /// HTTP 400 Bad Request if the provided ID is invalid.
        /// </returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PessoaDTO>> GetById(Guid id)
        {
            var pessoa = await _pessoaService.GetByIdAsync(id);

            if (pessoa is null)
                return NotFound();

            return Ok(pessoa);
        }

        /// <summary>
        /// Creates a new person with the provided data.
        /// </summary>
        /// <param name="cadastroPessoa"></param>
        /// <returns>
        /// HTTP 201 Created if the creation is successful.
        /// HTTP 400 Bad Request if the provided data is invalid.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Create(CadastroPessoaDTO cadastroPessoa)
        {
            await _pessoaService.CreateAsync(cadastroPessoa);
            return Created();
        }

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pessoaDTO"></param>
        /// <returns>
        /// HTTP 204 No Content if the update is successful.
        /// HTTP 404 Not Found if the person with the specified ID does not exist.
        /// HTTP 400 Bad Request if the provided data is invalid.
        /// </returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update(Guid id, PessoaDTO pessoaDTO)
        {
            await _pessoaService.UpdateAsync(id, pessoaDTO);
            return NoContent();
        }

        /// <summary>
        /// Deletes a person by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// HTTP 204 No Content if the deletion is successful.
        /// HTTP 404 Not Found if the person with the specified ID does not exist.
        /// HTTP 400 Bad Request if the provided ID is invalid.
        /// </returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _pessoaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
