using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class TipoHabitacionService : ITipoHabitacionService
    {
        private readonly ITipoHabitacionRepository _tipoHabitacionRepository;

        public TipoHabitacionService(
            ITipoHabitacionRepository tipoHabitacionRepository)
        {
            _tipoHabitacionRepository = tipoHabitacionRepository;
        }

        public List<TipoHabitacionDto> ObtenerTodos()
        {
            var tipos = _tipoHabitacionRepository.ObtenerTodos();

            return tipos.Select(t => new TipoHabitacionDto
            {
                TipoHabitacionId = t.TipoHabitacionId,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion,
                Capacidad = t.Capacidad,
                PrecioPorNoche = t.PrecioPorNoche
            }).ToList();
        }

        public TipoHabitacionDto? ObtenerPorId(int tipoHabitacionId)
        {
            var tipo = _tipoHabitacionRepository.ObtenerPorId(tipoHabitacionId);

            if (tipo == null)
                return null;

            return new TipoHabitacionDto
            {
                TipoHabitacionId = tipo.TipoHabitacionId,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                Capacidad = tipo.Capacidad,
                PrecioPorNoche = tipo.PrecioPorNoche
            };
        }

        public int Registrar(TipoHabitacionDto tipoHabitacion)
        {
            var entidad = new TipoHabitacion
            {
                Nombre = tipoHabitacion.Nombre,
                Descripcion = tipoHabitacion.Descripcion,
                Capacidad = tipoHabitacion.Capacidad,
                PrecioPorNoche = tipoHabitacion.PrecioPorNoche
            };

            return _tipoHabitacionRepository.Insertar(entidad);
        }

        public void Actualizar(TipoHabitacionDto tipoHabitacion)
        {
            var entidad = new TipoHabitacion
            {
                TipoHabitacionId = tipoHabitacion.TipoHabitacionId,
                Nombre = tipoHabitacion.Nombre,
                Descripcion = tipoHabitacion.Descripcion,
                Capacidad = tipoHabitacion.Capacidad,
                PrecioPorNoche = tipoHabitacion.PrecioPorNoche
            };

            _tipoHabitacionRepository.Actualizar(entidad);
        }

        public void Eliminar(int tipoHabitacionId)
        {
            _tipoHabitacionRepository.Eliminar(tipoHabitacionId);
        }
    }
}
