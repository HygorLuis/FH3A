using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PH3A.API.Interfaces;

namespace PH3A.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimulacoesController : ControllerBase
    {
        private readonly ISimulacaoService _simulacaoService;

        public SimulacoesController(ISimulacaoService simulacaoService)
        {
            _simulacaoService = simulacaoService;
        }

        [HttpGet]
        [Route("buscarSimulacoes/{qtd:int?}")]
        public async Task<IActionResult> BuscarSimulacoes(int qtd = 0)
        {
            var simulacoes = await _simulacaoService.BuscarSimulacoes(qtd);
            if (!simulacoes.Any())
                return NoContent();

            return Ok(simulacoes);
        }
    }
}
