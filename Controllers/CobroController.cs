using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Controllers
{
    public class CobroController : Controller
    {
        // GET: Cobro
        public ActionResult Index(string propietario, string estado, int? mes, int? anno)
        {
            List<SpConsultarCobrosResult> list = new List<SpConsultarCobrosResult>();
            List<SpConsultarClientesActivosResult> clientes = new List<SpConsultarClientesActivosResult>();
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCobros().ToList();
                    clientes = db.SpConsultarClientesActivos().ToList();

                    // Aplicar filtros
                    if (!string.IsNullOrEmpty(propietario))
                    {
                        list = list.Where(c => c.Persona.Contains(propietario)).ToList();
                    }
                    if (!string.IsNullOrEmpty(estado))
                    {
                        list = list.Where(c => c.Estado == estado).ToList();
                    }
                    if (mes.HasValue)
                    {
                        list = list.Where(c => c.Mes == mes.Value).ToList();
                    }
                    if (anno.HasValue)
                    {
                        list = list.Where(c => c.Anno == anno.Value).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                ModelState.AddModelError("", "Ocurrió un error al obtener los cobros: " + ex.Message);
            }

            
            ViewBag.Clientes = new SelectList(clientes, "id_persona", "Cliente");
            return View(list);
        }

        // Acción para filtrar cobros de manera asíncrona
        [HttpPost]
        public ActionResult FiltrarCobros(string propietario, string estado, int? mes, int? anno)
        {
            List<SpConsultarCobrosResult> list = new List<SpConsultarCobrosResult>();
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCobros().ToList();

                    // Aplicar filtros
                    if (!string.IsNullOrEmpty(propietario))
                    {
                        list = list.Where(c => c.Persona.Contains(propietario)).ToList();
                    }
                    if (!string.IsNullOrEmpty(estado))
                    {
                        list = list.Where(c => c.Estado == estado).ToList();
                    }
                    if (mes.HasValue)
                    {
                        list = list.Where(c => c.Mes == mes.Value).ToList();
                    }
                    if (anno.HasValue)
                    {
                        list = list.Where(c => c.Anno == anno.Value).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return Json(new { success = false, message = ex.Message });
            }

            return PartialView("_CobrosTable", list);
        }
    }
}