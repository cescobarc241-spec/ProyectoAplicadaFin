using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.DTOs
{
    public class EstanciaDto
    {
        public int EstanciaId { get; set; }

        public int ReservaId { get; set; }

        public DateTime FechaCheckIn { get; set; }

        public DateTime? FechaCheckOut { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string? Observaciones { get; set; }
    }
}
