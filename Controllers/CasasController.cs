using DataModels;
using G03_Sistema_Condominios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Controllers
{
    public class CasasController : Controller
    {
        // GET: Casas
        public ActionResult Index()
        {
            var list = new List<SpConsultarCasasResult>();
            //recordar que es new nombre de la BD en SQL
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.SpConsultarCasas().ToList();
            }
            //pasamos la lista a la vista
            return View(list);
        }

        public ActionResult CrearCasa()
        {
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                
                //puede que sea necesario modificar este sp o bien crear el correspondiente para tener acceso a las personas
                ViewBag.Clientes = db.SpConsultarClientesActivos().ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult CrearCasa(G03_Sistema_Condominios.Models.ModelCasa casa)
        {
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    db.SpCreaCasa(casa.NombreCasa,casa.MetrosCuadrados,casa.NumeroHabitaciones,casa.NumeroBanos,casa.Precion,casa.IdPersona,casa.FechaConstruccion,casa.Estado);
                    //puede que sea necesario modificar este sp o bien crear el correspondiente para tener acceso a las personas
                    ViewBag.Clientes = db.SpConsultarClientesActivos().ToList();
                }



            }
            catch
            {

            }

            return View();
        }
    }
}