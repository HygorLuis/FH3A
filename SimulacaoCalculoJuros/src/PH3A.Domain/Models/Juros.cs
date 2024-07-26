using PH3A.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace PH3A.Domain.Models
{
    public class Juros : ICalcularJuros
    {
        public Juros(int idParcela)
        {
            IdParcela = idParcela;
        }

        [Key]
        public int Id { get; set; }
        public int IdParcela { get; set; }
        public int IdSimulacao { get; set; }
        public decimal ValorJuros { get; private set; }

        public void Linear(Parcela parcela, decimal porcentagemJuros)
        {
            var atraso = (decimal)(parcela.DataVencimento < DateTime.Today ? (DateTime.Today - parcela.DataVencimento)?.Days : 0);
            ValorJuros = parcela.Valor.GetValueOrDefault() * porcentagemJuros * (atraso / 30);
        }

        public void Capitalizado(Parcela parcela, decimal porcentagemJuros)
        {
            var atraso = (decimal)(parcela.DataVencimento < DateTime.Today ? (DateTime.Today - parcela.DataVencimento)?.Days : 0);
            ValorJuros = parcela.Valor.GetValueOrDefault() * (decimal)(Math.Pow((double)(1 + porcentagemJuros), (double)(atraso / 30)) - 1);
        }
    }
}
