using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models.ViewModel
{
    public class CorredorViewModel
    {
        public int Id { get; set; }
        public string TituloCorrida { get; set; }
        [Display(Name = "Peso Inicial")]
        public double PesoIcinial { get; set; }
        [Display(Name = "Peso Atual")]
        public double PesoAtual { get; set; }
        [Display(Name = "Peso Objetivo")]
        public double PesoObjetivo { get; set; }
        public string Nome { get; set; }
        public bool Aprovado { get; set; }
        [Display(Name = "Peso Perdido")]
        public double PesoPerdido { get { return Math.Round(PesoIcinial - PesoAtual, 2); } }
        [Display(Name = "Falta Perder")]
        public double FaltaPerder { get { return Math.Round((PesoIcinial - PesoObjetivo) - PesoPerdido, 2); } }
    }
}