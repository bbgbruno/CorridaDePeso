using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models
{
    public class MetaCorredor
    {
        public int id { get; set; }
        public int CorredorId { get; set; }
        public int CorridaId { get; set; }
        public double ValorObjetivo { get; set; }
    }
}