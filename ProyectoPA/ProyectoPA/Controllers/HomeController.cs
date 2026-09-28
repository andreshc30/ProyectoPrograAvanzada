using System.Web.Mvc;

namespace ProyectoPA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
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
