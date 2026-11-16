using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoPA.EF;
using ProyectoPA.Models;


namespace ProyectoPA.Controllers
{
    public class CarritoController : Controller


    #region Consultar Carrito
    {
        [HttpGet]
        public ActionResult ConsultarCarrito()
        {
            try
            {
                FIDE_DBEntities context = new FIDE_DBEntities();
                {
                    var datos = context.SP_CONSULTAR_CARRITO().ToList();

                    var modelo = datos.Select(x => new CarritoModel
                    {
                        IdDiseno = x.ID_DISENO,
                        Imagen = x.IMAGEN,
                        Texto = x.TEXTO,
                        Material = x.MATERIAL,
                        PrecioMaterial = x.PRECIO_MATERIAL,
                        Tamano = x.TAMANO,
                        PrecioTamano = x.PRECIO_TAMANO,
                        Subtotal = x.PRECIO_MATERIAL + x.PRECIO_TAMANO,
                        IVA = (x.PRECIO_MATERIAL + x.PRECIO_TAMANO) * 0.13m,
                        Total = (x.PRECIO_MATERIAL + x.PRECIO_TAMANO) * 1.13m
                    }).ToList();

                    return View(modelo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

                return View(new List<CarritoModel>());
            }
        }

    #endregion

        #region Consultar Carrito
        [HttpGet]
        public ActionResult EliminarDiseno(int id)
        {
            try
            {
                FIDE_DBEntities context = new FIDE_DBEntities();

                var diseno = context.FIDE_DISENO_TB
                                    .Where(x => x.ID_DISENO == id)
                                    .FirstOrDefault();

                if (diseno != null)
                {
                    context.FIDE_DISENO_TB.Remove(diseno);

                    context.SaveChanges();
                }

                return RedirectToAction("ConsultarCarrito", "Carrito");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

                return RedirectToAction("ConsultarCarrito", "Carrito");
            }
        }

        #endregion


        #region Facrurar Pedido

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FacturarPedido()
        {
            try
            {
                FIDE_DBEntities context = new FIDE_DBEntities();

                var diseno = context.SP_CONSULTAR_CARRITO()
                                    .FirstOrDefault();

                if (diseno != null)
                {
                    decimal subtotal =
                        diseno.PRECIO_MATERIAL +
                        diseno.PRECIO_TAMANO;

                    decimal iva =
                        subtotal * 0.13m;

                    decimal total =
                        subtotal + iva;

                    var resultado =
                        context.SP_REGISTRAR_FACTURA(
                            "Placa Personalizada",
                            subtotal,
                            iva,
                            total,
                            diseno.ID_DISENO
                        ).FirstOrDefault();

                    return RedirectToAction(
                        "ConsultarFactura",
                        "Factura",
                        new
                        {
                            id = Convert.ToInt32(resultado)
                        });
                }

                ViewBag.Message =
                    "No existen diseños para facturar.";

                return RedirectToAction(
                    "ConsultarCarrito");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

                return RedirectToAction(
                    "ConsultarCarrito");
            }
        }



    }
        #endregion
}