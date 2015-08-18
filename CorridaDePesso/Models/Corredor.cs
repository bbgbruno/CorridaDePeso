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
        public Corredor()
        {
            Aprovado = false;
            //Corrida = new Corrida();
            Corridas = new List<Corrida>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        [Display (Name="Peso Inicial")]
        public double PesoIcinial { get; set; }
        [Display(Name = "Peso Atual")]
        public double PesoAtual { get; set; }
        [Display(Name = "Peso Objetivo")]
        public double PesoObjetivo { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Url da Imagem")]
        public String urlImagemCorredor { get; set; }
        public bool Aprovado { get; set; }
        public ICollection<Corrida> Corridas { get; set; }
        
        
        [Display(Name = "Peso Perdido")]
        [NotMapped]
        public double PesoPerdido { get { return Math.Round(PesoIcinial - PesoAtual, 2); } }
        [Display(Name = "Falta Perder")]
        [NotMapped]
        public double FaltaPerder { get { return Math.Round((PesoIcinial - PesoObjetivo) - PesoPerdido, 2); } }
        
    }
}