using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models
{
    public class Corrida
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public Guid UserId { get; set; }
        public string Regras { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public string EmailADM { get; set; }
        public bool Publica { get; set; }
    }
}