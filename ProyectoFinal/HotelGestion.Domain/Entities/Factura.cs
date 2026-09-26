using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Entities
{
    public class Factura
    {
        public int FacturaId { get; set; }

        public int EstanciaId { get; set; }

        public string NumeroFactura { get; set; } = string.Empty;

        public DateTime FechaEmision { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public string MetodoPago { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
}
