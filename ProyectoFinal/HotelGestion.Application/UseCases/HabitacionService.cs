using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class HabitacionService : IHabitacionService
    {
        private readonly IHabitacionRepository _habitacionRepository;

        public HabitacionService(IHabitacionRepository habitacionRepository)
        {
            _habitacionRepository = habitacionRepository;
        }

        public List<HabitacionDto> ObtenerTodos()
        {
            var habitaciones = _habitacionRepository.ObtenerTodos();

            return habitaciones.Select(h => new HabitacionDto
            {
                HabitacionId = h.HabitacionId,
                Numero = h.Numero,
                Piso = h.Piso,
                TipoHabitacionId = h.TipoHabitacionId,
                Estado = h.Estado,
                Descripcion = h.Descripcion
            }).ToList();
        }

        public HabitacionDto? ObtenerPorId(int habitacionId)
        {
            var habitacion = _habitacionRepository.ObtenerPorId(habitacionId);

            if (habitacion == null)
                return null;

            return new HabitacionDto
            {
                HabitacionId = habitacion.HabitacionId,
                Numero = habitacion.Numero,
                Piso = habitacion.Piso,
                TipoHabitacionId = habitacion.TipoHabitacionId,
                Estado = habitacion.Estado,
                Descripcion = habitacion.Descripcion
            };
        }

        public int Registrar(HabitacionDto habitacion)
        {
            var entidad = new Habitacion
            {
                Numero = habitacion.Numero,
                Piso = habitacion.Piso,
                TipoHabitacionId = habitacion.TipoHabitacionId,
                Estado = habitacion.Estado,
                Descripcion = habitacion.Descripcion
            };

            return _habitacionRepository.Insertar(entidad);
        }

        public void Actualizar(HabitacionDto habitacion)
        {
            var entidad = new Habitacion
            {
                HabitacionId = habitacion.HabitacionId,
                Numero = habitacion.Numero,
                Piso = habitacion.Piso,
                TipoHabitacionId = habitacion.TipoHabitacionId,
                Estado = habitacion.Estado,
                Descripcion = habitacion.Descripcion
            };

            _habitacionRepository.Actualizar(entidad);
        }

        public void Eliminar(int habitacionId)
        {
            _habitacionRepository.Eliminar(habitacionId);
        }
    }
}
