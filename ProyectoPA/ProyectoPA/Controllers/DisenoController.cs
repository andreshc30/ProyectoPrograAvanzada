using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ProyectoPA.EF;
using ProyectoPA.Models;

namespace ProyectoPA.Controllers
{
    public class DisenoController : Controller
    {
        #region Diseñar Placa

        [HttpGet]
        public ActionResult Index()
        {
            CargarOpciones();
            return View(new DisenoModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DisenoModel model)
        {
            try
            {
                string rutaImagen = null;

                if (model.ImagenArchivo != null && model.ImagenArchivo.ContentLength > 0)
                {
                    var extension = Path.GetExtension(model.ImagenArchivo.FileName).ToLower();
                    var permitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                    if (!permitidas.Contains(extension) || !model.ImagenArchivo.ContentType.StartsWith("image/"))
                    {
                        ViewBag.Message = "El archivo seleccionado no es una imagen válida.";
                        CargarOpciones();
                        return View(model);
                    }

                    var carpeta = Server.MapPath("~/assets/img/disenos/");
                    Directory.CreateDirectory(carpeta);

                    var nombreArchivo = "diseno_" + Guid.NewGuid().ToString("N") + extension;
                    model.ImagenArchivo.SaveAs(Path.Combine(carpeta, nombreArchivo));

                    rutaImagen = "/assets/img/disenos/" + nombreArchivo;
                }

                using (var context = new FIDE_DBEntities())
                {
                    context.FIDE_DISENO_TB.Add(new FIDE_DISENO_TB
                    {
                        ID_MATERIAL = model.IdMaterial,
                        ID_TAMANO = model.IdTamano,
                        TEXTO = model.Texto,
                        IMAGEN = rutaImagen,
                        ID_ESTADO = 1
                    });
                    var response = context.SaveChanges();

                    if (response > 0)
                        return RedirectToAction("ConsultarCarrito", "Carrito");
                }

                ViewBag.Message = "No se registró el diseño de la placa.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            CargarOpciones();
            return View(model);
        }

        #endregion

        private void CargarOpciones()
        {
            try
            {
                using (var context = new FIDE_DBEntities())
                {
                    ViewBag.Materiales = context.FIDE_MATERIAL_TB.Where(x => x.ID_ESTADO == 1).ToList();
                    ViewBag.Tamanos = context.FIDE_TAMANO_TB.Where(x => x.ID_ESTADO == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.Materiales = new List<FIDE_MATERIAL_TB>();
                ViewBag.Tamanos = new List<FIDE_TAMANO_TB>();
            }
        }
    }
}
