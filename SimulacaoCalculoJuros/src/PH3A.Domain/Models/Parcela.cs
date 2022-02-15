using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH3A.Domain.Models
{
    public class Parcela
    {
        public Parcela()
        {
            DataInclusao = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Parcela")]
        [Required(ErrorMessage = "Preencha o campo Parcela")]
        [Range(1, 99, ErrorMessage = "Apenas números entre 1 e 99")]
        public int? NumParc { get; set; }

        [Required(ErrorMessage = "Preencha o campo Valor")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Apenas valor maior que R$ 0,01")]
        public decimal? Valor { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Date de Vencimento no formato incorreto")]
        [Required(ErrorMessage = "Preencha o campo Data de Vencimento")]
        [Display(Name = "Data de Vencimento")]
        public DateTime? DataVencimento { get; set; }

        [Display(Name = "Data da Inclusão")]
        public DateTime DataInclusao { get; set; }
    }
}
