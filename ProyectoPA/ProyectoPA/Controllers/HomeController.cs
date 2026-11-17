using ProyectoPA.EF;
using ProyectoPA.Models;
using ProyectoPA.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProyectoPA.Controllers
{
    public class HomeController : Controller
    {
        UsuarioService usuarioService = new UsuarioService();

        public ActionResult Index()
        {
            return View();
        }

        #region Dashboard
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

        #endregion

        #region Inicio de Sesión

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsuarioModel model)
        {
            try
            {
                using (var context = new FIDE_DBEntities())
                {
                    var response = context.SP_INICIAR_SESION(model.Correo, model.Contrasena).FirstOrDefault();

                    if (response != null)
                    {
                        Session["IdUsuario"] = response.ID_USUARIO;
                        Session["NombreUsuario"] = response.NOMBRE + " " + response.PRIMER_APELLIDO;
                        Session["NombreRol"] = response.NOMBRE_ROL;
                        return RedirectToAction("Dashboard", "Home");
                    }
                }

                ViewBag.Message = "No se autenticó su información, consulte con el administrador.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Se presentó un error, consulte con el administrador.";
            }

            return View();
        }

        #endregion

        #region Registro de Usuarios

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(UsuarioModel model)
        {
            try
            {
                using (var context = new FIDE_DBEntities())
                {

                    var response = context.SP_REGISTRAR_USUARIO(model.Cedula, model.Nombre, model.PrimerApellido, model.SegundoApellido, model.Correo, model.Contrasena);

                    if (response > 0)
                        return RedirectToAction("Login", "Home");
                }

                ViewBag.Message = "No se registró su información, consulte con el administrador.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Se presentó un error, consulte con el administrador.";
            }

            return View();
        }

        #endregion

        #region Olvido de Contraseña

        [HttpGet]
        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Recuperar(UsuarioModel model)
        {
            try
            {
                using (var context = new FIDE_DBEntities())
                {
                    var response = context.FIDE_USUARIO_TB.Where(x => x.CEDULA == model.Cedula
                                                          && x.ID_ESTADO == 1).FirstOrDefault();

                    if (response != null)
                    {
                        var userIdCifrado = usuarioService.Cifrar(response.ID_USUARIO, TimeSpan.FromHours(1));
                        var link = ConfigurationManager.AppSettings["UrlSistema"] + "Home/ChangePassword/" + userIdCifrado;

                        //Envío del correo con un mensaje que diga que ese código expira en 1 hora

                        var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "recursos", "RecuperarContrasena.html");

                        var html = System.IO.File.ReadAllText(ruta, Encoding.UTF8)
                            .Replace("{{Nombre}}", WebUtility.HtmlEncode(response.NOMBRE + " " + response.PRIMER_APELLIDO + " " + response.SEGUNDO_APELLIDO))
                            .Replace("{{Link}}", WebUtility.HtmlEncode(link))
                            .Replace("{{Anio}}", DateTime.Now.Year.ToString());

                        await usuarioService.EnviarCorreo(response.CORREO, "Recuperación de Contraseña", html);
                        return RedirectToAction("Login", "Home");
                    }
                }

                ViewBag.Message = "No se validó su información, consulte con el administrador.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Se presentó un error, consulte con el administrador.";
            }

            return View();
        }

        #endregion

        #region Cambiar Contraseña

        [HttpGet]
        public ActionResult CambiarContrasena(string id)
        {
            //Se guarda el id en la sesión
            Session["IdUsuarioCifrado"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult CambiarContrasena(UsuarioModel model)
        {
            try
            {
                //Se lee el id de la sesión y se valida
                var idUsuarioDescrifrado = usuarioService.Descifrar(Session["IdUsuarioCifrado"].ToString());

                if (idUsuarioDescrifrado == null)
                {
                    ViewBag.Message = "El enlace utilizado no es válido o ya expiró.";
                    return View();
                }

                using (var context = new FIDE_DBEntities())
                {
                    var response = context.FIDE_USUARIO_TB.Where(x => x.ID_USUARIO == idUsuarioDescrifrado).FirstOrDefault();

                    if (response != null)
                    {
                        response.CONTRASENA = model.Contrasena;
                        var updateResponse = context.SaveChanges();

                        if (updateResponse > 0)
                            return RedirectToAction("Login", "Home");
                    }
                }

                ViewBag.Message = "No se actualizó su información, consulte con el administrador.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Se presentó un error, consulte con el administrador.";
            }

            return View();
        }

        #endregion


        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

    }
}
