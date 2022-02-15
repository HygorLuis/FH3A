using System.Collections.Generic;
using System.Threading.Tasks;
using PH3A.Domain.Models;

namespace PH3A.API.Interfaces
{
    public interface ISimulacaoService
    {
        Task<List<Simulacao>> BuscarSimulacoes(int qtd);
    }
}
