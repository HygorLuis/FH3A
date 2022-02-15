using PH3A.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PH3A.Domain.Interfaces
{
    public interface ISimulacaoService
    {
        Task<int> IncluirSimulacao(Simulacao simulacao);
        Task<List<Simulacao>> BuscarSimulacoes(int qtd);
    }
}
