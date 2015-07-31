using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CorridaDePesso.Models
{
    public class Pesagem
    {
        public Pesagem()
        {
            Data = DateTime.Today;
        }

        public int id { get; set; }
        [Display(Name="Corredor")]
        public int CorredorId { get; set; }
        virtual public Corredor Corredor { get; set; }
        public int CorridaId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Data { get; set; }
        public double Peso { get; set; }
        public string UserId { get; set; }
    }
}