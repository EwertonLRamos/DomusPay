using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DomusPay.Api.Controllers
{
    [Route("api/transacao")]
    [ApiController]
    public class TransacaoController(ITransacaoService transacaoService) : ControllerBase
    {
        private readonly ITransacaoService _transacaoService = transacaoService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<CadastroTransacaoDTO>>> GetAll()
        {
            var transacoes = await _transacaoService.GetAllAsync();

            if (transacoes is null || !transacoes.Any())
                return NotFound();

            return Ok(transacoes);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Create(CadastroTransacaoDTO transacaoDTO)
        {
            await _transacaoService.CreateAsync(transacaoDTO);
            return Created();
        }
    }
}
