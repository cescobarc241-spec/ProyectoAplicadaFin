using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Entities
{
    public class ProductoMinibar
    {
        public int ProductoMinibarId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}
