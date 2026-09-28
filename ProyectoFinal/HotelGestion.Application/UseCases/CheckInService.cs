using System;
using System.Collections.Generic;
using System.Text;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

namespace HotelGestion.Application.UseCases;

public class CheckInService : ICheckInService
{
    private readonly ICheckInRepository _checkInRepository;

    public CheckInService(ICheckInRepository checkInRepository)
    {
        _checkInRepository = checkInRepository;
    }

    public int Ejecutar(CheckInDto checkIn)
    {
        if (checkIn.ClienteId <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar un cliente.");
        }

        if (checkIn.HabitacionId <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar una habitación.");
        }

        if (checkIn.FechaSalida.Date <= checkIn.FechaCheckIn.Date)
        {
            throw new ArgumentException(
                "La fecha de salida debe ser posterior a la fecha de check-in.");
        }

        if (checkIn.CantidadHuespedes <= 0)
        {
            throw new ArgumentException(
                "La cantidad de huéspedes debe ser mayor que cero.");
        }

        if (checkIn.ConReserva && checkIn.ReservaId <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar una reserva.");
        }

        return _checkInRepository.Ejecutar(checkIn);
    }
}
