using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.DTOs
{
    public class HabitacionDto
    {
        public int HabitacionId { get; set; }

        public string Numero { get; set; } = string.Empty;

        public int Piso { get; set; }

        public int TipoHabitacionId { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}
