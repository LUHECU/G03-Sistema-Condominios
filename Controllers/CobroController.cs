using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;

namespace G03_Sistema_Condominios.Controllers
{
    public class CobroController : Controller
    {
        // GET: Cobro
        public ActionResult Index()
        {
            var list = new List<SpConsultarCobrosResult>();
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCobros().ToList();

                }
            }
            catch { }
            return View(list);

        }
    }
}
