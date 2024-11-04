using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Models
{
    public class ModelCobroView
    {
        public ModelCobro Cobro { get; set; }
        public List<SpConsultarServiciosResult> Servicios { get; set; }


    }
}