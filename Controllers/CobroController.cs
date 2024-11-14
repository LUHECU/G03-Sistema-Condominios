using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;
using G03_Sistema_Condominios.Models;
using DataModels;

namespace G03_Sistema_Condominios.Controllers
{
    public class CobroController : Controller
    {
        // GET: Cobro
        [HttpGet]
        public ActionResult Index()
        {
            var list = new List<SpConsultarCobrosResult>();
            try
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCobros().ToList();
                    
                }
            }
            catch{ }

            return View(list);
        }

        public ActionResult CrearModificarCobro() 
        {
            var cobro = new ModelCobro();
            var servicios = new List<SpConsultarServiciosResult>();
            var cobroView = new ModelCobroView();

            //Se cargan las listas de meses y años
            cobroView.annos = new List<int>() {2024,2025,2026,2027,2028,2029,2030,2031,2032,2033,2034};
            cobroView.meses  = new List<string>() {"Enero", "Febrero", "Marzo", "Mayo", "Abril", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};



            try 
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    servicios = db.SpConsultarServicios().ToList();
                    cobroView.Servicios = servicios;
                    cobroView.Cobro = cobro;

                }
            }
            catch { }

            return View(cobroView);
        }

        //[HttpPost]
        //public ActionResult CrearModificarCobro(Cobro cobro)
        //{

        //    try
        //    {
        //        using (var db = new PviProyectoFinalDB("MyDatabase"))
        //        {
        //            if (cobro.IdCobro == 0)
        //            {
        //                db.SpCrearCobro(cobro.IdCasa, cobro.Mes, cobro.Anno);
        //            }
        //            else 
        //            {

        //            }
        //        }
        //    }
        //    catch { }

        //    return View();
        //}


        public JsonResult Clientes()
        {
            var list = new List<Dropdown>();
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarClientesActivos().Select(_ => new Dropdown 
                    {
                        Id = _.Id_persona,
                        Nombre = _.Cliente
                    }).ToList();
                }
            }
            catch{ }

            return Json(list);
        }

        public JsonResult Casas(int? idCliente)
        {
            var list = new List<Dropdown>();
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCasasActivasPorCliente(idCliente).Select(_ => new Dropdown
                    {
                        Id = _.Id_casa,
                        Nombre = _.Nombre_casa
                    }).ToList();
                }
            }
            catch { }

            return Json(list);
        }


    }
}