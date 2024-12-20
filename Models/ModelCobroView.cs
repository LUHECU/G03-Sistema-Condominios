﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Models
{
    public class ModelCobroView
    {
        public ModelCobro Cobro { get; set; }
        public List<SpConsultarServiciosResult> Servicios { get; set; }
        public List<SpConsultarCobrosResult> CobrosList { get; set; }
        public List<SpConsultarCobroPorClienteResult> CobrosClienteList { get; set; }
        public List<SpConsultarDetallePorIdCobroResult> DetalleCobro { get; set; }
        public List<SpConsultarBitacoraResult> Bitacora { get; set; }
        public List<int> annos { get; set; }
        public List<string> meses { get; set; }
        public List<string> CheckedServicios { get; set; }
        public ModelLogin Login { get; set; }
        public ModelUsuario Usuario { get; set; }
    }
}