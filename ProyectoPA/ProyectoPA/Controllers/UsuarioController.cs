
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ProyectoPA.Models;
using ProyectoPA.Services;

namespace ProyectoPA.Controllers
{
     [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController()
        {
            _usuarioService = new UsuarioService();
        }

        // MOSTRAR USUARIOS
        [HttpGet]
        public ActionResult MostrarUsuarios()
        {
            var usuarios = _usuarioService.ListarUsuarios();
            return View(usuarios);
        }

        // EDITAR USUARIO
        [HttpGet]
        public ActionResult EditarUsuario(int id)
        {
            var usuario = _usuarioService.ObtenerUsuario(id);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            CargarListas();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(UsuarioModel modelo)
        {
            if (!ModelState.IsValid)
            {
                CargarListas();
                return View(modelo);
            }

            try
            {
                int filas = _usuarioService.EditarUsuario(modelo);

                if (filas == 0)
                {
                    return HttpNotFound();
                }

                TempData["Mensaje"] =
                    "Usuario actualizado correctamente.";

                return RedirectToAction("MostrarUsuarios");
            }
            catch (SqlException)
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo actualizar el usuario. " +
                    "Verifique que la cédula y el correo " +
                    "no estén duplicados."
                );

                CargarListas();
                return View(modelo);
            }
        }

        // ADMINISTRAR ROLES DE UN USUARIO
        [HttpGet]
        public ActionResult AdministrarRoles(int id)
        {
            var usuario = _usuarioService.ObtenerUsuario(id);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            ViewBag.Usuario = usuario;

            var roles = _usuarioService.ListarRoles();

            var idEstadoActivo = _usuarioService.ObtenerEstados()
                .Where(e => e.ESTADO == "Activo")
                .Select(e => e.ID_ESTADO)
                .FirstOrDefault();

            ViewBag.RolesDisponibles = new SelectList(
                roles.Where(r => r.ID_ESTADO == idEstadoActivo),
                "ID_ROL",
                "NOMBRE"
            );

            var rolesUsuario =
                _usuarioService.ObtenerRolesUsuario(id);

            return View(rolesUsuario);
        }

        // ASIGNAR ROL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarRol(int idUsuario, int idRol)
        {
            try
            {
                _usuarioService.AsignarRol(idUsuario, idRol);

                TempData["Mensaje"] =
                    "Rol asignado correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] =
                    "No se pudo asignar el rol.";
            }

            return RedirectToAction(
                "AdministrarRoles",
                new { id = idUsuario }
            );
        }

        // QUITAR ROL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuitarRol(int idUsuario, int idRol)
        {
            try
            {
                _usuarioService.QuitarRol(idUsuario, idRol);

                TempData["Mensaje"] =
                    "Rol quitado correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] =
                    "No se pudo quitar el rol. " +
                    "Verifique que no sea el último administrador.";
            }

            return RedirectToAction(
                "AdministrarRoles",
                new { id = idUsuario }
            );
        }

        // DESACTIVAR USUARIO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DesactivarUsuario(int id)
        {
            try
            {
                _usuarioService.DesactivarUsuario(id);

                TempData["Mensaje"] =
                    "Usuario desactivado correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] =
                    "No se pudo desactivar el usuario. " +
                    "Verifique que no sea el último administrador.";
            }

            return RedirectToAction("MostrarUsuarios");
        }

        // CARGAR ESTADOS Y DIRECCIONES
        private void CargarListas()
        {
            var estados = _usuarioService.ObtenerEstados();

            ViewBag.Estados = new SelectList(
                estados,
                "ID_ESTADO",
                "ESTADO"
            );

            var direcciones = _usuarioService.ObtenerDirecciones()
                .Select(d => new
                {
                    d.ID_DIRECCION,
                    Descripcion = d.OTROS_DETALLES
                })
                .ToList();

            ViewBag.Direcciones = new SelectList(
                direcciones,
                "ID_DIRECCION",
                "Descripcion"
            );
        }
    }
}
