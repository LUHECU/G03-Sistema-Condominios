using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Controllers
{
    public class ServiciosController : Controller
    {
        // GET: Servicios
        public ActionResult Index()
        {
            var list = new List<Servicio>();
            //recordar que es new nombre de la BD en SQL
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.SpConsultarServicios().ToList();
            }
            //pasamos la lista a la vista

            return View(list);
        }
    }
}