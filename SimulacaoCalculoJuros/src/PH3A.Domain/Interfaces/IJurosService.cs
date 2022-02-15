using System.Collections.Generic;
using System.Threading.Tasks;
using PH3A.Domain.Models;

namespace PH3A.Domain.Interfaces
{
    public interface IJurosService
    {
        Task IncluirJuros(List<Juros> juros, int idSimulacao);
    }
}
