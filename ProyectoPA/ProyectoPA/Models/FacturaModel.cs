using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPA.Models
{
    public class FacturaModel
    {
        public int IdFactura { get; set; }

        public DateTime FechaEmision { get; set; }

        public string Detalle { get; set; }

        public string Texto { get; set; }

        public decimal Total { get; set; }
    }
}