
using System.ComponentModel.DataAnnotations;

namespace ProyectoPA.Models
{
    public class CrearUsuarioModel
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        public string CEDULA { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NOMBRE { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string PRIMER_APELLIDO { get; set; }

        public string SEGUNDO_APELLIDO { get; set; }

        public string PERFIL_URL { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string CORREO { get; set; }

        [Required(ErrorMessage = "Seleccione un estado")]
        public int? ID_ESTADO { get; set; }

        [Required(ErrorMessage = "Seleccione una dirección")]
        public int? ID_DIRECCION { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string CONTRASENA { get; set; }

        [Required(ErrorMessage = "Confirme la contraseña")]
        [DataType(DataType.Password)]
        [Compare("CONTRASENA",
            ErrorMessage = "Las contraseñas no coinciden")]
        public string CONFIRMAR_CONTRASENA { get; set; }
    }
}
