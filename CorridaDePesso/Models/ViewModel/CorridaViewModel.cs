using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models
{
    public class CorridaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public int NumeroCorredores { get; set; }
        public Corredor CorredorLider { get; set; }
        public string Titulo { get; set; }
        public bool Publica { get; set; }
        public string Email { get; set; }
    }
}