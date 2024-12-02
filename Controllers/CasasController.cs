using DataModels;
using G03_Sistema_Condominios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
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

           try
            {
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
                        FechaConstruccion = x.Fecha_construccion,
                        Estado = (bool)x.Estado
                    }).FirstOrDefault();

                    //puede que sea necesario modificar este sp o bien crear el correspondiente para tener acceso a las personas
                    ViewBag.Clientes = db.SpConsultarClientesActivos().ToList();

                    // Verifica si la casa existe al colocar un ID por el url
                    if (casa == null)
                    {
                        ViewBag.Resultado = "La casa no existe, por favor cree la casa nueva";
                        ViewBag.Clientes = db.SpConsultarClientesActivos().ToList(); // Carga nuevamente los datos de clientes
                        return View(casa); // Devuelve la vista con el mensaje
                    }

                    // Si la casa está inactiva, muestra un mensaje y redirige a otra página
                    if (casa != null && !casa.Estado)
                    {
                        TempData["Resultado"] = "La casa seleccionada está inactiva y no puede ser modificada.";
                        return RedirectToAction("Index"); // Redirige a Index
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Resultado"] = "Ocurrió un error al cargar el la casa: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View(casa);
        }

        [HttpPost]
        public ActionResult CrearCasa(G03_Sistema_Condominios.Models.ModelCasa casa)
        {
            var resultado = string.Empty;

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    //puede que sea necesario modificar este sp o bien crear el correspondiente para tener acceso a las personas
                    ViewBag.Clientes = db.SpConsultarClientesActivos().ToList();

                    if (casa.IdCasa == 0) {

                        db.SpCreaCasa(casa.NombreCasa, casa.MetrosCuadrados, casa.NumeroHabitaciones, casa.NumeroBanos, casa.IdPersona, casa.FechaConstruccion);
                        resultado = "Se ha creado una nueva casa exitosamente";
                    }
                    else
                    {
                        //verificar si la casa tiene servicio activos y cobros pendientes asociados 
                        var cobrosPendientes = db.SpVerificarServiciosCobrosPendientes(casa.IdCasa).FirstOrDefault();

                        if( cobrosPendientes != null && cobrosPendientes.Column1 == 1)
                        {
                            // Si hay servicios y cobros pendientes, se muestra un mensaje y no se realiza la operación
                            ViewBag.Resultado = "La casa tiene servicios activos y/ cobros pendientes y no puede ser modificada.";
                            ViewBag.Clientes = db.SpConsultarClientesActivos().ToList();
                            return View(casa); // Vuelve a la misma vista con el mensaje
                        }
                        else
                        {
                            db.SpModificarCasa(casa.IdCasa, casa.NombreCasa, casa.MetrosCuadrados, casa.NumeroHabitaciones, casa.NumeroBanos, casa.IdPersona, casa.FechaConstruccion);
                            ViewBag.Resultado = "Se ha modificado la casa exitosamente";
                            return View(casa);
                        }
                       
                    }

                   
                }

            }
            catch
            {
                resultado = "No se ha guardado exitosamente la información";
            }

            ViewBag.Resultado = resultado;
            return View();
        }

        public ActionResult InactivarCasa(int? idCasa)
        {
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                //verificar el estado de la casa 
                var casa = db.SpConsultarCasasPorID(idCasa).FirstOrDefault();

                if (casa != null && casa.Estado == false)
                {
                    ViewBag.Resultado = "La casa ya está inactiva.";
                    return RedirectToAction("Index");
                }

                db.SpInactivarCasa(idCasa);
            }

            return RedirectToAction("Index");

        }
    }
}