using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models.ViewModel
{
   public enum TipoVerificacao
    {
        [Display(Name = "Peso Total")]
        Peso = 0,
        [Display(Name ="Percentual de Gordura" )]
        PercentuaGordura = 2,
        [Display(Name = "Circunferencia da Cintura")]
        CircunferenciaContura
        
    }
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
        [Display(Name = "Url da Imagem")]
        public String urlImagemCorredor { get; set; }
        public virtual Corrida Corrida { get; set; }
        public int CorridaId { get; set; }
        public string Email { get; set; }
        [Display(Name = "Acompanhamento")]
        public TipoVerificacao TipoAcompanhamento { get; set; }
        public double ValorObjetivo { get; set; }
    }
}