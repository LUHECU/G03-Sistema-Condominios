using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G03_Sistema_Condominios.Models
{
    public class ModelCobro
    {
        public int IdCobro { get; set; }
        public int IdCasa { get; set; }
        public string NumCasa { get; set; }
        public string Persona { get; set; }
        public int mes {  get; set; }
        public int anno { get; set; }
        public string estado { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaPagada { get; set; }
    }
}