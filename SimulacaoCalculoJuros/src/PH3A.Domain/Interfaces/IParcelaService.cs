using System.Collections.Generic;
using System.Threading.Tasks;
using PH3A.Domain.Models;

namespace PH3A.Domain.Interfaces
{
    public interface IParcelaService
    {
        Task<List<Parcela>> BuscarParcelas(int id = 0);
        Task<Parcela> IncluirParcela(Parcela parcela);
        Task<Parcela> AlterarParcela(Parcela parcela);
        Task ExcluirParcela(int id);
    }
}
