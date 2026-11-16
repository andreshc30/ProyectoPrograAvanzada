using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProyectoPA.EF;

namespace ProyectoPA.Controllers
{
    public class CatalogoController : Controller
    {
        #region Consultar Catálogo

        [HttpGet]
        public ActionResult Index()
        {
            var placas = new List<FIDE_PLACA_TB>();

            try
            {
                using (var context = new FIDE_DBEntities())
                {
                    placas = context.FIDE_PLACA_TB
                                    .Where(x => x.DISPONIBLE)
                                    .ToList();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View(placas);
        }

        #endregion
    }
}
