
using System.ComponentModel.DataAnnotations;

namespace ProyectoPA.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public string Cedula { get; set; }

        public string Nombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Correo { get; set; }

        public string Contrasena { get; set; }

        public string ConfirmarContrasena { get; set; }

        public int IdEstado { get; set; }
        
        public int IdDireccion { get; set; }

        public string PerfilUrl { get; set; }
    }
}
