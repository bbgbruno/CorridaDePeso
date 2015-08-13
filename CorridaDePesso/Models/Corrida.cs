using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models
{
    public enum TipoCalculo
    {
        [Display(Name = "Perda De Peso")]
        PerdaDePeso = 0,
        [Display(Name = "Ganho De Peso")]
        AumentoDePeso = 1
    }

    public enum TipoEvolucao
    {
        Percentual = 0,
        [Display(Name = "Valor Fixo")]
        ValorFixo = 1
    }

    public class Corrida
    {
        public Corrida()
        {
            Participantes = new List<Corredor>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Regras { get; set; }
        public TipoCalculo TipoCorrida { get; set; }
        public TipoEvolucao Evolucao { get; set; }
        [Display(Name = "Descrição da Corrida")]
        public string Titulo { get; set; }
        [Display(Name = "Informe o Valor ou Percentual")]
        public double FatorCalculo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Data de Inicio")]
        public DateTime DataInicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Data de Termino")]
        public DateTime DataFinal { get; set; }
        [Display(Name = "Email do Adm da Corrida")]
        public string EmailADM { get; set; }
        [Display(Name = "é Pùblica")]
        public bool Publica { get; set; }
        public string Link { get; set; }
        public ICollection<Corredor> Participantes { get; set; }
    }
}