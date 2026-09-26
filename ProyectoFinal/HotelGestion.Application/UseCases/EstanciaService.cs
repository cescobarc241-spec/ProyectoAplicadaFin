using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class EstanciaService : IEstanciaService
    {
        private readonly IEstanciaRepository _estanciaRepository;

        public EstanciaService(IEstanciaRepository estanciaRepository)
        {
            _estanciaRepository = estanciaRepository;
        }

        public List<EstanciaDto> ObtenerTodos()
        {
            var estancias = _estanciaRepository.ObtenerTodos();

            return estancias.Select(e => new EstanciaDto
            {
                EstanciaId = e.EstanciaId,
                ReservaId = e.ReservaId,
                FechaCheckIn = e.FechaCheckIn,
                FechaCheckOut = e.FechaCheckOut,
                Estado = e.Estado,
                Observaciones = e.Observaciones
            }).ToList();
        }

        public EstanciaDto? ObtenerPorId(int estanciaId)
        {
            var estancia = _estanciaRepository.ObtenerPorId(estanciaId);

            if (estancia == null)
                return null;

            return new EstanciaDto
            {
                EstanciaId = estancia.EstanciaId,
                ReservaId = estancia.ReservaId,
                FechaCheckIn = estancia.FechaCheckIn,
                FechaCheckOut = estancia.FechaCheckOut,
                Estado = estancia.Estado,
                Observaciones = estancia.Observaciones
            };
        }

        public int Registrar(EstanciaDto estancia)
        {
            var entidad = new Estancia
            {
                ReservaId = estancia.ReservaId,
                FechaCheckIn = estancia.FechaCheckIn,
                FechaCheckOut = estancia.FechaCheckOut,
                Estado = estancia.Estado,
                Observaciones = estancia.Observaciones
            };

            return _estanciaRepository.Insertar(entidad);
        }

        public void Actualizar(EstanciaDto estancia)
        {
            var entidad = new Estancia
            {
                EstanciaId = estancia.EstanciaId,
                ReservaId = estancia.ReservaId,
                FechaCheckIn = estancia.FechaCheckIn,
                FechaCheckOut = estancia.FechaCheckOut,
                Estado = estancia.Estado,
                Observaciones = estancia.Observaciones
            };

            _estanciaRepository.Actualizar(entidad);
        }

        public void Eliminar(int estanciaId)
        {
            _estanciaRepository.Eliminar(estanciaId);
        }
    }
}
