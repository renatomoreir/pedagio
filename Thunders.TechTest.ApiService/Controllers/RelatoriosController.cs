using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Services;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly RelatorioService _service;

        public RelatoriosController(RelatorioService service)
        {
            _service = service;
        }

        [HttpGet("total-por-hora")]
        public async Task<IActionResult> GetTotalPorHora()
            => Ok(await _service.TotalPorHoraPorCidade());

        [HttpGet("top-pracas")]
        public async Task<IActionResult> GetTopPracas([FromQuery] int quantidade = 5)
            => Ok(await _service.PracasQueMaisFaturaramPorMes(quantidade));

        [HttpGet("veiculos-por-praca")]
        public async Task<IActionResult> GetVeiculosPorPraca([FromQuery] string praca)
            => Ok(await _service.TiposDeVeiculosPorPraca(praca));
    }
}
