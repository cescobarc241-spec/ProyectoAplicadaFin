using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.DTOs
{
    public class ConsumoMinibarDto
    {
        public int ConsumoMinibarId { get; set; }

        public int EstanciaId { get; set; }

        public int ProductoMinibarId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public DateTime FechaConsumo { get; set; }
    }
}
