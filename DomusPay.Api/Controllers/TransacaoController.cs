using DomusPay.Application.DTOs;
using DomusPay.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DomusPay.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar as operações relacionadas às transações, como criação e consulta de transações.
    /// </summary>
    /// <param name="transacaoService"></param>
    [Route("api/transacao")]
    [ApiController]
    public class TransacaoController(ITransacaoService transacaoService) : ControllerBase
    {
        private readonly ITransacaoService _transacaoService = transacaoService;

        /// <summary>
        /// Retrieves a list of all transactions along with their details.
        /// </summary>
        /// <returns>
        /// HTTP 200 OK with the list of transactions if found.
        /// HTTP 404 Not Found if no transactions are found.
        /// HTTP 400 Bad Request if the request is invalid.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ListagemBaseDTO<CadastroTransacaoDTO>>> GetAll()
        {
            var transacoes = await _transacaoService.GetAllAsync();

            if (transacoes is null || transacoes.Itens.Count == 0)
                return NotFound();

            return Ok(transacoes);
        }

        /// <summary>
        /// Creates a new transaction with the provided data.
        /// </summary>
        /// <param name="transacaoDTO"></param>
        /// <returns>
        /// HTTP 201 Created if the creation is successful.
        /// HTTP 400 Bad Request if the provided data is invalid.
        /// </returns>
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
