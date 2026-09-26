using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Entities
{
    public class Reserva
    {
        public int ReservaId { get; set; }

        public int ClienteId { get; set; }

        public int HabitacionId { get; set; }

        public DateTime FechaReserva { get; set; }

        public DateTime FechaEntrada { get; set; }

        public DateTime FechaSalida { get; set; }

        public int CantidadHuespedes { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string? Observaciones { get; set; }
    }
}
