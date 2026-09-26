using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Entities
{
    public class TipoHabitacion
    {
        public int TipoHabitacionId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public int Capacidad { get; set; }

        public decimal PrecioPorNoche { get; set; }
    }
}
