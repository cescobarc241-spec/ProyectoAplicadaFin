using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class ConsumoMinibarService : IConsumoMinibarService
    {
        private readonly IConsumoMinibarRepository _consumoMinibarRepository;

        public ConsumoMinibarService(
            IConsumoMinibarRepository consumoMinibarRepository)
        {
            _consumoMinibarRepository = consumoMinibarRepository;
        }

        public List<ConsumoMinibarDto> ObtenerTodos()
        {
            var consumos = _consumoMinibarRepository.ObtenerTodos();

            return consumos.Select(c => new ConsumoMinibarDto
            {
                ConsumoMinibarId = c.ConsumoMinibarId,
                EstanciaId = c.EstanciaId,
                ProductoMinibarId = c.ProductoMinibarId,
                Cantidad = c.Cantidad,
                PrecioUnitario = c.PrecioUnitario,
                FechaConsumo = c.FechaConsumo
            }).ToList();
        }

        public ConsumoMinibarDto? ObtenerPorId(int consumoMinibarId)
        {
            var consumo =
                _consumoMinibarRepository.ObtenerPorId(consumoMinibarId);

            if (consumo == null)
                return null;

            return new ConsumoMinibarDto
            {
                ConsumoMinibarId = consumo.ConsumoMinibarId,
                EstanciaId = consumo.EstanciaId,
                ProductoMinibarId = consumo.ProductoMinibarId,
                Cantidad = consumo.Cantidad,
                PrecioUnitario = consumo.PrecioUnitario,
                FechaConsumo = consumo.FechaConsumo
            };
        }

        public int Registrar(ConsumoMinibarDto consumo)
        {
            var entidad = new ConsumoMinibar
            {
                EstanciaId = consumo.EstanciaId,
                ProductoMinibarId = consumo.ProductoMinibarId,
                Cantidad = consumo.Cantidad,
                PrecioUnitario = consumo.PrecioUnitario,
                FechaConsumo = consumo.FechaConsumo
            };

            return _consumoMinibarRepository.Insertar(entidad);
        }

        public void Actualizar(ConsumoMinibarDto consumo)
        {
            var entidad = new ConsumoMinibar
            {
                ConsumoMinibarId = consumo.ConsumoMinibarId,
                EstanciaId = consumo.EstanciaId,
                ProductoMinibarId = consumo.ProductoMinibarId,
                Cantidad = consumo.Cantidad,
                PrecioUnitario = consumo.PrecioUnitario,
                FechaConsumo = consumo.FechaConsumo
            };

            _consumoMinibarRepository.Actualizar(entidad);
        }

        public void Eliminar(int consumoMinibarId)
        {
            _consumoMinibarRepository.Eliminar(consumoMinibarId);
        }
    }
}
