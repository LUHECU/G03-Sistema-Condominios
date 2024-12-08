using DataModels;
using G03_Sistema_Condominios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Data.SqlClient;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Controllers
{
    public class ServiciosController : Controller
    {
        // GET: Servicios

        // Acción para mostrar la lista de servicios
        public ActionResult Index()
        {
            //Verificador de inicio de sesión
            if (Session["UserId"] == null)
            {
                //si no hay un usuario con sesión activa, redirecciona al login para que ingrese credenciales
                return RedirectToAction("Login", "Login");
            }

            //Modelo de modelos 
            var cobroView = new ModelCobroView();

            
            var list = new List<SpConsultarServiciosResult>();
            // Conexión con la base de datos para obtener los servicios
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                // Consulta los servicios y los guarda en la lista
                list = db.SpConsultarServicios().ToList();
                //Se incorpora al modelo de modelos
                cobroView.Servicios = list;
            }

            //pasamos la lista a la vista
            return View(cobroView);
        }

        // Acción para crear o modificar un servicio
        public ActionResult CrearServicio(int? idServicio)
        {
            //Verificador de inicio de sesión
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var servicio = new ModelServicio();  

           try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    // Validar si el idServicio existe, en caso de buscarse por el URL directamente
                    var servicioExistente = db.SpConsultarServiciosPorID(idServicio).FirstOrDefault();

                    if (idServicio.HasValue && servicioExistente == null)
                    {
                        // Si el idServicio no existe, redirigir al Index con un mensaje
                        TempData["Resultado"] = "El servicio que está intentando encontrar o modificar no existe.";
                        return RedirectToAction("Index");
                    }

                    // se convierten los datos del servicio a un modelo para la vista
                    servicio = db.SpConsultarServiciosPorID(idServicio).Select(x => new ModelServicio
                    {
                        IdServicio = x.IdServicio,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        Precio = x.Precio,
                        IdCategoria = x.IdCategoria,
                        Estado = x.Estado

                    }).FirstOrDefault();

                    // Carga la lista de categorías para el dropdown en la vista
                    ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();                

                    // Si el servicio está inactivo, muestra un mensaje o redirige a otra página
                    if (servicio != null && !servicio.Estado)
                    {
                        TempData["Resultado"] = "El servicio seleccionado está inactivo y no puede ser modificado.";
                        return RedirectToAction("Index"); // Redirige a Index
                    }

                    return View(servicio);
                }
            }
            catch(Exception ex)
            {
                // Manejo de errores al cargar el servicio
                TempData["Resultado"] = "Ocurrió un error al cargar el servicio: " + ex.Message;
                return RedirectToAction("Index");
            }           

        }

        // Acción para guardar un servicio (crear o modificar)
        [HttpPost]
        public ActionResult CrearServicio(ModelServicio servicio)
        {
            //Guardar el mensaje de resultado de la acción crear o modificar servicio
            var resultado = string.Empty;

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    // Carga la lista de categorías para la vista
                    ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();

                    //Logica para crear o modificar el servicio 
                    // Si no se proporciona un ID, crea un nuevo servicio
                    if (servicio.IdServicio == 0)
                    {
                        db.SpCreaServicios(servicio.Nombre, servicio.Descripcion, servicio.Precio, servicio.IdCategoria);

                        resultado = "Se ha guardado exitosamente";

                    }
                    else //si viene un idServicio entonces ejecute el proceso para modificar
                    {
                        // Verificar si el servicio tiene cobros pendientes antes de guardar
                        var tienePendientes = db.SpVerificarPendientesServicio(servicio.IdServicio).FirstOrDefault();
                        //Si tienependientes es igual a 1 quiere decir que hay un registro de cobro asociado
                        if (tienePendientes != null && tienePendientes.Column1 == 1)
                        {
                            // Si hay cobros pendientes, se muestra un mensaje y no se realiza la operación
                            ViewBag.Resultado = "El servicio tiene cobros pendientes y no puede ser modificado.";
                            ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();
                            return View(servicio); // Vuelve a la misma vista con el mensaje
                        }
                        else
                        {
                            // Modifica el servicio existente
                            db.SpModificarServicios(servicio.IdServicio, servicio.Nombre, servicio.Descripcion, servicio.Precio, servicio.IdCategoria);
                            ViewBag.Resultado = "Se ha modificado exitosamente";
                            return View(servicio);
                        }

                    }

                }
            }
            catch(Exception ex) 
            {
                // Verificar si el error es causado por un nombre o categoría repetida
                //Se hace consulta directa al RAISERROR del procedimiento almacenado
                if (ex.Message.Contains("Ya existe un servicio con el mismo nombre en esta categoría."))
                {
                    ViewBag.Resultado = "El nombre del servicio ya existe con esa categoría. Por favor, elija otro nombre y/o categoría.";
                    return View();
                }
                // Manejo de errores al guardar el servicio
                resultado = "No se ha guardado exitosamente";
            }

            ViewBag.Resultado = resultado;
            return View();
        }

        // Acción para inactivar un servicio
        public ActionResult InactivarServicio(int? idServicio)
        {
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    // Verificar si el servicio ya está inactivo
                    //esto es única y exclusivamente si por alguna razón la primer validación en la carga de 
                    //los formularios falla al identificar el estado de un servicio
                    var servicio = db.SpConsultarServiciosPorID(idServicio).FirstOrDefault();

                    if (servicio != null && !servicio.Estado)
                    {
                        ViewBag.Resultado = "El servicio ya está inactivo.";
                        return RedirectToAction("Index");
                    }
                    // Inactiva el servicio
                    db.SpInactivarServicio(idServicio);
                }

                //Muestre el mensaje de resultado en el Index al redirigir la página 
                TempData["Resultado"] = "El servicio ha sido inactivada correctamente.";
                return RedirectToAction("Index");
            }
            catch
            {
                //Manejo de errores
                TempData["Resultado"] = "Error al inactivar el servicio";
                return RedirectToAction("Index");
            }
   
        }
    }
}