using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Models
{
    public class LoginCobros
    {

        public class ModelCobroView
        {
            public Login Login { get; set; }
            public List<SpConsultarCobrosResult> Cobro { get; set; }
        }
    }
}