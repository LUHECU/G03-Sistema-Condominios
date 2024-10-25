using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G03_Sistema_Condominios.Models
{
    public class ModelServicio
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int IdCategoria { get; set; }    
        public bool Estado { get; set; }

    }
}