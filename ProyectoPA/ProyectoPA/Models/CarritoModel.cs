using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPA.Models
{
    public class CarritoModel
    {

        public int IdDiseno { get; set; }

        public string Imagen { get; set; }

        public string Texto { get; set; }

        public string Material { get; set; }

        public decimal PrecioMaterial { get; set; }

        public string Tamano { get; set; }

        public decimal PrecioTamano { get; set; }

        public decimal Subtotal { get; set; }

        public decimal IVA { get; set; }

        public decimal Total { get; set; }
    }
}