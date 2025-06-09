using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.Services;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilizacoesController : ControllerBase
    {
        private readonly UtilizacaoService _utilizacaoService;

        public UtilizacoesController(UtilizacaoService utilizacaoService)
        {
            _utilizacaoService = utilizacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizacao>>> GetUtilizacoes()
        {
            var utilizacoes = await _utilizacaoService.GetAllUtilizacoesAsync();
            return Ok(utilizacoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizacao>> GetUtilizacao(int id)
        {
            var utilizacao = await _utilizacaoService.GetUtilizacaoByIdAsync(id);

            if (utilizacao == null)
            {
                return NotFound();
            }

            return Ok(utilizacao);
        }

        [HttpPost]
        public async Task<ActionResult<Utilizacao>> PostUtilizacao([FromBody] Utilizacao utilizacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUtilizacao = await _utilizacaoService.AddUtilizacaoAsync(utilizacao);

            return CreatedAtAction(nameof(GetUtilizacao), new { id = newUtilizacao.Id }, newUtilizacao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizacao(int id, [FromBody] Utilizacao utilizacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _utilizacaoService.UpdateUtilizacaoAsync(id, utilizacao);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizacao(int id)
        {
            var success = await _utilizacaoService.DeleteUtilizacaoAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}