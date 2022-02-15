using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PH3A.Domain.Enums;
using PH3A.Domain.Interfaces;
using PH3A.Domain.Models;

namespace PH3A.Site.Controllers
{
    public class SimuladorController : Controller
    {
        private readonly IParcelaService _parcelaService;
        private readonly IJurosService _jurosService;
        private readonly ISimulacaoService _simulacaoService;

        public SimuladorController(IParcelaService parcelaService, ISimulacaoService simulacaoService, IJurosService jurosService)
        {
            _parcelaService = parcelaService;
            _jurosService = jurosService;
            _simulacaoService = simulacaoService;
        }

        public async Task<IActionResult> Index()
        {
            var parcelas = await _parcelaService.BuscarParcelas();
            return View(parcelas);
        }

        #region Parcela

        [HttpGet]
        public async Task<IActionResult> ListarParcelas()
        {
            var parcelas = await _parcelaService.BuscarParcelas();
            return View("_ListagemParcelas", parcelas);
        }

        #region Cadastro

        [HttpGet]
        public async Task<IActionResult> CadastroParcela(int id)
        {
            var parcelas = id > 0 ? (await _parcelaService.BuscarParcelas(id)).FirstOrDefault() : new Parcela();
            return PartialView("_CadastroParcela", parcelas);
        }

        [HttpPost]
        public async Task<IActionResult> IncluirParcela(Parcela parcela)
        {
            if (!ModelState.IsValid) return PartialView("_CadastroParcela", parcela);

            var parcelas = await _parcelaService.BuscarParcelas();
            if (parcelas.Any(x => x.DataVencimento == parcela.DataVencimento))
            {
                ModelState.AddModelError("DataVencimento", "Vencimento já cadastrado em outra parcela");
                return PartialView("_CadastroParcela", parcela);
            }

            parcela = await _parcelaService.IncluirParcela(parcela);
            parcelas.Add(parcela);

            return PartialView("_ListagemParcelas", parcelas);
        }

        [HttpPut]
        public async Task<IActionResult> AlterarParcela(Parcela parcela)
        {
            if (!ModelState.IsValid) return PartialView("_CadastroParcela", parcela);

            var parcelas = await _parcelaService.BuscarParcelas();
            if (parcelas.Any(x => x.Id != parcela.Id && x.DataVencimento == parcela.DataVencimento))
            {
                ModelState.AddModelError("DataVencimento", "Vencimento já cadastrado em outra parcela");
                return PartialView("_CadastroParcela", parcela);
            }

            parcela = await _parcelaService.AlterarParcela(parcela);
            var novaParcela = parcelas.FirstOrDefault(x => x.Id == parcela.Id);
            novaParcela.NumParc = parcela.NumParc;
            novaParcela.Valor = parcela.Valor;
            novaParcela.DataVencimento = parcela.DataVencimento;

            return PartialView("_ListagemParcelas", parcelas);
        }

        #endregion

        #region Excluir

        [HttpDelete]
        public async Task<IActionResult> ExcluirParcela(int id)
        {
            await _parcelaService.ExcluirParcela(id);
            var parcelas = await _parcelaService.BuscarParcelas();

            return PartialView("_ListagemParcelas", parcelas);
        }

        #endregion

        #endregion

        #region Calcular Juros

        [HttpGet]
        public IActionResult CarregarJuros()
        {
            return PartialView("_CalcularJuros");
        }

        [HttpPost]
        public async Task<IActionResult> CalcularJurosParcela(Simulacao simulacao)
        {
            if (!ModelState.IsValid) return PartialView("_CalcularJuros", simulacao);

            var parcelas = await _parcelaService.BuscarParcelas();
            if (!parcelas.Any()) return PartialView("_CalcularJuros", simulacao);

            var juros = new List<Juros>();

            parcelas.ForEach(x =>
            {
                juros.Add(new Juros
                {
                    IdParcela = x.Id,
                    ValorJuros = CalcularTipoJuros(x, simulacao)
                });
            });

            simulacao.TotalJuros = juros.Sum(x => x.ValorJuros);
            simulacao.TotalDivida = parcelas.Sum(x => x.Valor.GetValueOrDefault()) + simulacao.TotalJuros;

            var id = await _simulacaoService.IncluirSimulacao(simulacao);
            if (id > 0)
                await _jurosService.IncluirJuros(juros, id);

            return PartialView("_CalcularJuros", simulacao);
        }

        private decimal CalcularTipoJuros(Parcela parcela, Simulacao simulacao)
        {
            decimal valorJuros = 0;
            var atraso = (decimal)(parcela.DataVencimento < DateTime.Today ? (DateTime.Today - parcela.DataVencimento)?.Days : 0);

            switch (simulacao.TipoCalculo)
            {
                case TipoJuros.Linear:
                    valorJuros = parcela.Valor.GetValueOrDefault() * simulacao.PorcentagemJuros.GetValueOrDefault() * (atraso / 30);
                    break;

                case TipoJuros.Capitalizado:
                    valorJuros = parcela.Valor.GetValueOrDefault() * (decimal)(Math.Pow((double)(1 + simulacao.PorcentagemJuros.GetValueOrDefault()), (double)(atraso / 30)) - 1);
                    break;
            }

            return valorJuros;
        }

        #endregion
    }
}
