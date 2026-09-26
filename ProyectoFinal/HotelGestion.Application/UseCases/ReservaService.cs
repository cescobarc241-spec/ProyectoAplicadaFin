using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaService(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        public List<ReservaDto> ObtenerTodos()
        {
            var reservas = _reservaRepository.ObtenerTodos();

            return reservas.Select(r => new ReservaDto
            {
                ReservaId = r.ReservaId,
                ClienteId = r.ClienteId,
                HabitacionId = r.HabitacionId,
                FechaReserva = r.FechaReserva,
                FechaEntrada = r.FechaEntrada,
                FechaSalida = r.FechaSalida,
                CantidadHuespedes = r.CantidadHuespedes,
                Estado = r.Estado,
                Observaciones = r.Observaciones
            }).ToList();
        }

        public ReservaDto? ObtenerPorId(int reservaId)
        {
            var reserva = _reservaRepository.ObtenerPorId(reservaId);

            if (reserva == null)
                return null;

            return new ReservaDto
            {
                ReservaId = reserva.ReservaId,
                ClienteId = reserva.ClienteId,
                HabitacionId = reserva.HabitacionId,
                FechaReserva = reserva.FechaReserva,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                CantidadHuespedes = reserva.CantidadHuespedes,
                Estado = reserva.Estado,
                Observaciones = reserva.Observaciones
            };
        }

        public int Registrar(ReservaDto reserva)
        {
            var entidad = new Reserva
            {
                ClienteId = reserva.ClienteId,
                HabitacionId = reserva.HabitacionId,
                FechaReserva = reserva.FechaReserva,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                CantidadHuespedes = reserva.CantidadHuespedes,
                Estado = reserva.Estado,
                Observaciones = reserva.Observaciones
            };

            return _reservaRepository.Insertar(entidad);
        }

        public void Actualizar(ReservaDto reserva)
        {
            var entidad = new Reserva
            {
                ReservaId = reserva.ReservaId,
                ClienteId = reserva.ClienteId,
                HabitacionId = reserva.HabitacionId,
                FechaReserva = reserva.FechaReserva,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                CantidadHuespedes = reserva.CantidadHuespedes,
                Estado = reserva.Estado,
                Observaciones = reserva.Observaciones
            };

            _reservaRepository.Actualizar(entidad);
        }

        public void Eliminar(int reservaId)
        {
            _reservaRepository.Eliminar(reservaId);
        }
    }
}
