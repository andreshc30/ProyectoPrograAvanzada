using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoPA.EF;
using ProyectoPA.Models;

namespace ProyectoPA.Controllers
{
    public class FacturaController : Controller
    {

        [HttpGet]
        public ActionResult ConsultarFactura(int idDiseno)
        {
            try
            {
                FIDE_DBEntities context = new FIDE_DBEntities();

                var datos = context.SP_CONSULTAR_CARRITO()
                                   .Where(x => x.ID_DISENO == idDiseno)
                                   .FirstOrDefault();

                if (datos == null)
                {
                    ViewBag.Message = "No se encontró la factura.";
                    return View();
                }

                FacturaModel factura = new FacturaModel
                {
                    FechaEmision = DateTime.Now,
                    Detalle = "Placa Personalizada",
                    Texto = datos.TEXTO,
                    Total = datos.PRECIO_MATERIAL + datos.PRECIO_TAMANO
                };

                return View(factura);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

                return View();
            }
        }

    }
}