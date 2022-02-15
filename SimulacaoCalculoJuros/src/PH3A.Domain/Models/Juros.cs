using System.ComponentModel.DataAnnotations;

namespace PH3A.Domain.Models
{
    public class Juros
    {
        [Key]
        public int Id { get; set; }
        public int IdParcela { get; set; }
        public int IdSimulacao { get; set; }
        public decimal ValorJuros { get; set; }
    }
}
