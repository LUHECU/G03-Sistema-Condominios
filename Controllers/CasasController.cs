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

        public ActionResult CrearCasa(int? idCasa)
        {
            var casa = new ModelCasa();

            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                casa = db.SpConsultarCasasPorID(idCasa).Select(x => new ModelCasa
                {
                    IdCasa = x.Id_casa,
                    NombreCasa = x.Nombre_casa,
                    MetrosCuadrados = x.Metros_cuadrados,
                    NumeroHabitaciones = x.Numero_habitaciones,
                    NumeroBanos = x.Numero_banos,   
                    IdPersona = x.Id_persona,
                    FechaConstruccion = x.Fecha_construccion
                }).FirstOrDefault();
                
                //puede que sea necesario modificar este sp o bien crear el correspondiente para tener acceso a las personas
                ViewBag.Clientes = db.SpConsultarClientesActivos().ToList();
            }

            return View(casa);
        }

        [HttpPost]
        public ActionResult CrearCasa(G03_Sistema_Condominios.Models.ModelCasa casa)
        {
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    if (casa.IdCasa == 0) {

                        db.SpCreaCasa(casa.NombreCasa, casa.MetrosCuadrados, casa.NumeroHabitaciones, casa.NumeroBanos, casa.IdPersona, casa.FechaConstruccion);
                        
                    }
                    else
                    {
                        db.SpModificarCasa(casa.IdCasa,casa.NombreCasa,casa.MetrosCuadrados,casa.NumeroHabitaciones,casa.NumeroBanos,casa.IdPersona,casa.FechaConstruccion);
                    }

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