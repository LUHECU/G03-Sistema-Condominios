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


            try 
            {
                using(var db = new PviProyectoFinalDB("MyDatabse"))
                {

                    ViewBag.servicios = db.SpConsultarServicios().ToList();



                }
            }
            catch { }

            return View();
        }

        //[HttpPost]
        //public ActionResult CrearModificarCobro(Cobro cobro)
        //{

        //    try
        //    {
        //        using (var db = new PviProyectoFinalDB("MyDatabse"))
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

    }
}