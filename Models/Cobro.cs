using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G03_Sistema_Condominios.Models
{
    public class Cobro
    {
        public int IdCobro { get; set; } // Identificador 
        public int IDCasa { get; set; } // Identificador casa
        public string mes { get; set; } // Mes
        public string anno { get; set; } // Año
        public string estado { get; set; } // Estado
        public decimal monto { get; set; } // Monto
        public DateTime fecha_pagada { get; set; } // Fecha Pago
    }
}