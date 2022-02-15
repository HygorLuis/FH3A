using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PH3A.Domain.Enums;

namespace PH3A.Domain.Models
{
    public class Simulacao
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Juros")]
        [Required(ErrorMessage = "Preencha o campo Juros")]
        [Range(0.001, 999.999, ErrorMessage = "Apenas valores entre R$ 0,001 e R$ 999,999")]
        [Column(TypeName = "double(6,3)")]
        public decimal? PorcentagemJuros { get; set; }

        [Display(Name = "Tipo de Calculo")]
        public TipoJuros TipoCalculo { get; set; }

        [Display(Name = "Total de Juros")]
        public decimal TotalJuros { get; set; }

        [Display(Name = "Total da Dívida")]
        public decimal TotalDivida { get; set; }

        [Display(Name = "Data da Inclusão")]
        public DateTime DataInclusao { get; set; }
    }
}
