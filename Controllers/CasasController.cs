using DataModels;
using G03_Sistema_Condominios.Models;
using LinqToDB.SqlQuery;
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
            //Verificador de inicio de sesión
            if (Session["UserId"] == null)
            {
                //si no hay un usuario con sesión activa, redirecciona al login para que ingrese credenciales
                return RedirectToAction("Login", "Login");
            }

            //Verificador de rol de usuario
            if (Session["UserTipo"].Equals("Cliente"))
            {
                return RedirectToAction("Index", "Cobro");
            }

            // Acción para mostrar la lista de casas

            var list = new List<SpConsultarCasasResult>();
            // Conexión a la base de datos para obtener la lista de casas
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                list = db.SpConsultarCasas().ToList();

            }
            //pasamos la lista a la vista y devuelve la vista con la lista de casas
            return View(list);
        }

        // Acción para crear o modificar una casa
        public ActionResult CrearCasa(int? idCasa)
        {
            //Verificador de inicio de sesión
            if (Session["UserId"] == null)
            {
                //si no hay un usuario con sesión activa, redirecciona al login para que ingrese credenciales
                return RedirectToAction("Login", "Login");
            }

            //Verificador de rol de usuario
            if (Session["UserTipo"].Equals("Cliente"))
            {
                return RedirectToAction("Index", "Cobro");
            }

            var casa = new ModelCasa();

           try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    // Validar si el idCasa existe, en caso de que se busque por el URL directamente
                    var casaExistente = db.SpConsultarCasasPorID(idCasa).FirstOrDefault();

                    if (idCasa.HasValue && casaExistente == null)
                    {
                        // Si el idCasa no existe, redirigir al Index con un mensaje
                        TempData["Resultado"] = "La casa que está intentando encontrar o modificar no existe.";
                        return RedirectToAction("Index");
                    }

                    // Asigna los datos de la casa al modelo
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

                    // Validación para verificar si el usuario es el propietario de la casa a modificar/inactivar
                    if (casa != null && casa.IdPersona == int.Parse(Session["UserId"].ToString()))
                    {
                        TempData["Resultado"] = "No puede modificar una casa de la que es propietario. Solicite a otro empleado realizar esta acción.";
                        return RedirectToAction("Index");
                    }

                    // Filtrar clientes activos excluyendo al usuario actual
                    //Creación de variables locales para almacenar el ID del usuario con sesión activa en el momento
                    //y otra variable para hacer una selección de los clientes activos cuyo ID sea diferente al del usuario en sesión
                    var userId = int.Parse(Session["UserId"].ToString());
                    var clientesActivos = db.SpConsultarClientesActivos()
                                            .Where(cliente => cliente.Id_persona != userId)
                                            .ToList();

                    // Se pasa la lista de clientes activos excluyendo al usuario actual
                    ViewBag.Clientes = clientesActivos;
                }

                // Si la casa está inactiva, muestra un mensaje y redirige a otra página
                if (casa != null && !casa.Estado)
                    {
                        TempData["Resultado"] = "La casa seleccionada está inactiva y no puede ser modificada.";
                        return RedirectToAction("Index"); // Redirige a Index
                    }

                }           
            catch (Exception ex)
            {
                // Manejo de errores y redirige al índice
                TempData["Resultado"] = "Ocurrió un error al cargar el la casa: " + ex.Message;
                return RedirectToAction("Index");
            }
            // Devuelve la vista con los datos de la casa
            return View(casa);
        }

        // Acción POST para guardar una casa
        [HttpPost]
        public ActionResult CrearCasa(G03_Sistema_Condominios.Models.ModelCasa casa)
        {
            //Guardar el mensaje de resultado de la acción crear o modificar casa
            var resultado = string.Empty;

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    // Obtiene la lista de clientes activos
                    ViewBag.Clientes = db.SpConsultarClientesActivos().ToList();

                    //Si el ID es igual a 0 significa que debe mostrarse la vista para crear una casa
                    if (casa.IdCasa == 0) {
                        // Procedimiento almacenado que crea una nueva casa
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
                            // Modifica la casa existente
                            db.SpModificarCasa(casa.IdCasa, casa.NombreCasa, casa.MetrosCuadrados, casa.NumeroHabitaciones, casa.NumeroBanos, casa.IdPersona, casa.FechaConstruccion);
                            ViewBag.Resultado = "Se ha modificado la casa exitosamente";
                            return View(casa);
                        }
                       
                    }

                   
                }

            }
            catch(Exception ex) 
            {
                // Verificar si el error es causado por un nombre repetido
                //Se hace consulta directa al RAISERROR del procedimiento almacenado
                if (ex.Message.Contains("Ya existe una casa con el mismo nombre"))
                {
                    ViewBag.Resultado = "El nombre de la casa ya existe. Por favor, elija otro nombre.";
                    return View();
                }

                // Manejo general de errores
                resultado = "No se ha guardado la información debido a un error inesperado.";
            }

            //Pasamos el mensaje de la variable local resultado al viewbag correspondiente para ser mostrado en la vista
            ViewBag.Resultado = resultado;
            return View();
        }

        // Acción para inactivar una casa
        public ActionResult InactivarCasa(int? idCasa)
        {
            try
            { 
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                //verificar el estado de la casa 
                var casa = db.SpConsultarCasasPorID(idCasa).FirstOrDefault();

                    // Si ya está inactiva, muestra un mensaje, 
                    //esto es única y exclusivamente si por alguna razón la primer validación en la carga de 
                    //los formularios falla al identificar el estado de una casa 
                    if (casa != null && casa.Estado == false)
                {
                    ViewBag.Resultado = "La casa ya está inactiva.";
                    return RedirectToAction("Index");
                }
                    //si la casa esta activa puede ser inactivada mediante el procedimiento almacenado

                    //verificar si la casa tiene servicio activos y cobros pendientes asociados 
                    var cobrosPendientes = db.SpVerificarServiciosCobrosPendientes(idCasa).FirstOrDefault();

                    if (cobrosPendientes != null && cobrosPendientes.Column1 == 1)
                    {
                        //Muestre el mensaje de resultado en el Index al redirigir la página 
                        TempData["Resultado"] = "La casa tiene servicios activos y/ cobros pendientes y no puede ser inactivada.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //Muestre el mensaje de resultado en el Index al redirigir la página 
                        db.SpInactivarCasa(idCasa);
                        TempData["Resultado"] = "La casa ha sido inactivada correctamente.";
                        return RedirectToAction("Index");
                    }
                   
            }
            
            }
            catch
            {
                //Manejo de errores
                TempData["Resultado"] = "Error al inactivar la casa";
                return RedirectToAction("Index");
            }

        }
    }
}