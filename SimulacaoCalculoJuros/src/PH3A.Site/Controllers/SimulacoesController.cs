using System.Threading.Tasks;
using PH3A.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PH3A.Site.Controllers
{
    public class SimulacoesController : Controller
    {
        private readonly IConfiguration _configuration;
        private SimulacaoService _simulacaoService;

        public SimulacoesController(IConfiguration config)
        {
            _configuration = config;
            _simulacaoService = new SimulacaoService(_configuration);
        }

        public async Task<IActionResult> Index()
        {
            var simulacoes = await _simulacaoService.BuscarSimulacoes(10);
            return View(simulacoes);
        }
    }
}
