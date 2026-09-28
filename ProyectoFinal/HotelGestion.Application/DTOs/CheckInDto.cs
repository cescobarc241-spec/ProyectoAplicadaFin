using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.DTOs;

public class CheckInDto
{
    public int ClienteId { get; set; }

    public int ReservaId { get; set; }

    public int HabitacionId { get; set; }

    public DateTime FechaCheckIn { get; set; } = DateTime.Now;

    public DateTime FechaSalida { get; set; } = DateTime.Today.AddDays(1);

    public int CantidadHuespedes { get; set; } = 1;

    public string? Observaciones { get; set; }

    public bool ConReserva { get; set; }
}