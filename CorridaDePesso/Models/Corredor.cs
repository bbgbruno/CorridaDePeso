using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models
{
    public class Corredor
    {
        public int id { get; set; }
        [Display (Name="Peso Inicial")]
        public double PesoIcinial { get; set; }
        [Display(Name = "Peso Atual")]
        public double PesoAtual { get; set; }
        [Display(Name = "Peso Objetivo")]
        public double PesoObjetivo { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Peso Perdido")]
        [Display(Name = "Url da Imagem")]
        public String urlImagemCorredor { get; set; }
        public int CorridaId { get; set; }
        public virtual Corrida Corrida { get; set; }
        [NotMapped]
        public double PesoPerdido { get { return Math.Round(PesoIcinial - PesoAtual, 2); } }
 
    }
}