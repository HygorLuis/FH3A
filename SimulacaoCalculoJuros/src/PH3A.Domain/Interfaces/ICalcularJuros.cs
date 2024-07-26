using PH3A.Domain.Models;

namespace PH3A.Domain.Interfaces
{
    public interface ICalcularJuros
    {
        void Linear(Parcela parcela, decimal porcentagemJuros);
        void Capitalizado(Parcela parcela, decimal porcentagemJuros);
    }
}
