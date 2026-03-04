using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DomusPay.Api.Controllers
{
    [Route("api/pessoa")]
    [ApiController]
    public class PessoaController(
        IPessoaService pessoaService
    ) : ControllerBase
    {
        private readonly IPessoaService _pessoaService = pessoaService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<PessoaComValoresTotaisDTO>>> GetAll()
        {
            var pessoas = await _pessoaService.GetAllAsync();

            if (pessoas is null || !pessoas.Any())
                return NotFound();

            return Ok(pessoas);
        }

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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Create(PessoaDTO pessoaDTO)
        {
            await _pessoaService.CreateAsync(pessoaDTO);
            return CreatedAtAction(nameof(GetById), new { id = pessoaDTO.Id }, pessoaDTO);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update(Guid id, PessoaDTO pessoaDTO)
        {
            await _pessoaService.UpdateAsync(id, pessoaDTO);
            return NoContent();
        }

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
