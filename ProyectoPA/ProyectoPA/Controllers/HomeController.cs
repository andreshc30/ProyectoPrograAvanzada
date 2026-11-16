using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProyectoPA.EF;

namespace ProyectoPA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            var destacados = new List<FIDE_PLACA_TB>();

            try
            {
                using (var context = new FIDE_DBEntities())
                {
                    destacados = context.FIDE_PLACA_TB
                                        .Where(x => x.DISPONIBLE)
                                        .OrderByDescending(x => x.ID_PLACA)
                                        .Take(4)
                                        .ToList();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View(destacados);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        public ActionResult Recuperar()
        {
            return View();
        }
        public ActionResult Service()
        {
            return View();
        }

        public ActionResult Price()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult Team()
        {
            return View();
        }

        public ActionResult Testimonial()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // Página 404 de la plantilla
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
