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

        public ActionResult CrearServicio(int? idServicio)
        {
            var servicio = new ModelServicio();  

           try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    servicio = db.SpConsultarServiciosPorID(idServicio).Select(x => new ModelServicio
                    {
                        IdServicio = x.IdServicio,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        Precio = x.Precio,
                        IdCategoria = x.IdCategoria,
                        Estado = x.Estado

                    }).FirstOrDefault();
                
                    ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();

                    // Verifica si el servicio existe al colocar un ID en el URL
                    if (servicio == null)
                    {
                        ViewBag.Resultado = "el servicio no existe, por favor cree el nuevo servicio";
                        ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList(); // Carga nuevamente los datos de las categorías
                        return View(servicio); // Devuelve la vista con el mensaje
                    }

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
                TempData["Resultado"] = "Ocurrió un error al cargar el servicio: " + ex.Message;
                return RedirectToAction("Index");
            }           

           // return View(servicio);
        }

        [HttpPost]
        public ActionResult CrearServicio(ModelServicio servicio)
        {
            var resultado = string.Empty;

            //if (servicio.Precio == 0)
            //{
            //    // Log para depurar
            //    System.Diagnostics.Debug.WriteLine("El valor de Precio es: " + servicio.Precio);
            //}

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();

                    //Logica para crear o modificar el servicio 
                    if (servicio.IdServicio == 0)
                    {
                        db.SpCreaServicios(servicio.Nombre, servicio.Descripcion, servicio.Precio, servicio.IdCategoria);

                        //ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();

                        resultado = "Se ha guardado exitosamente";

                    }
                    else //si viene un idServicio entonces ejecute el proceso para modificar
                    {
                        // Verificar si el servicio tiene cobros pendientes antes de guardar
                        var tienePendientes = db.SpVerificarPendientesServicio(servicio.IdServicio).FirstOrDefault();

                        if (tienePendientes != null && tienePendientes.Column1 == 1)
                        {
                            // Si hay cobros pendientes, se muestra un mensaje y no se realiza la operación
                            ViewBag.Resultado = "El servicio tiene cobros pendientes y no puede ser modificado.";
                            ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();
                            return View(servicio); // Vuelve a la misma vista con el mensaje
                        }
                        else
                        {
                            db.SpModificarServicios(servicio.IdServicio, servicio.Nombre, servicio.Descripcion, servicio.Precio, servicio.IdCategoria);
                            ViewBag.Resultado = "Se ha modificado exitosamente";
                            return View(servicio);
                        }

                    }

                   // ViewBag.Categorias = db.SpConsultarCategoriasServicios().ToList();

                }
            }
            catch
            {
                resultado = "No se ha guardado exitosamente";
            }

            ViewBag.Resultado = resultado;
            return View();
        }

        public ActionResult InactivarServicio(int? idServicio)
        {
            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {
                // Verificar si el servicio ya está inactivo
                var servicio = db.SpConsultarServiciosPorID(idServicio).FirstOrDefault();
                if (servicio != null && !servicio.Estado)
                {
                    ViewBag.Resultado = "El servicio ya está inactivo.";
                    return RedirectToAction("Index");
                }

                db.SpInactivarServicio(idServicio);
            }
            return RedirectToAction("Index");
        }
    }
}