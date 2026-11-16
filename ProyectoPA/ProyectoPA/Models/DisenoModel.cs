using System.Web;

namespace ProyectoPA.Models
{
    public class DisenoModel
    {
        public int IdMaterial { get; set; }
        public int IdTamano { get; set; }
        public string Texto { get; set; }
        public HttpPostedFileBase ImagenArchivo { get; set; }
    }
}
