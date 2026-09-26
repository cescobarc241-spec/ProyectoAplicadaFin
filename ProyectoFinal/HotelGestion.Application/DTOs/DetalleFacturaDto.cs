using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.DTOs
{
    public class DetalleFacturaDto
    {
        public int DetalleFacturaId { get; set; }

        public int FacturaId { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}
