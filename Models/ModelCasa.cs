using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G03_Sistema_Condominios.Models
{
    public class ModelCasa
    {
        public int IdCasa { get; set; }
        public string NombreCasa { get; set; }
        public int MetrosCuadrados { get; set; }
        public int NumeroHabitaciones { get; set; }
        public int NumeroBanos { get; set; }
        public decimal Precion { get; set; }
        public int IdPersona { get; set; }
        public DateTime FechaConstruccion { get; set; }
        public bool Estado { get; set; }

    }
}