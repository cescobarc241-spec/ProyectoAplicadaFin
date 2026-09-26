using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class DetalleFacturaService : IDetalleFacturaService
    {
        private readonly IDetalleFacturaRepository _detalleFacturaRepository;

        public DetalleFacturaService(
            IDetalleFacturaRepository detalleFacturaRepository)
        {
            _detalleFacturaRepository = detalleFacturaRepository;
        }

        public List<DetalleFacturaDto> ObtenerTodos()
        {
            var detalles = _detalleFacturaRepository.ObtenerTodos();

            return detalles.Select(d => new DetalleFacturaDto
            {
                DetalleFacturaId = d.DetalleFacturaId,
                FacturaId = d.FacturaId,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList();
        }

        public DetalleFacturaDto? ObtenerPorId(int detalleFacturaId)
        {
            var detalle =
                _detalleFacturaRepository.ObtenerPorId(detalleFacturaId);

            if (detalle == null)
                return null;

            return new DetalleFacturaDto
            {
                DetalleFacturaId = detalle.DetalleFacturaId,
                FacturaId = detalle.FacturaId,
                Descripcion = detalle.Descripcion,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Subtotal
            };
        }

        public int Registrar(DetalleFacturaDto detalle)
        {
            var entidad = new DetalleFactura
            {
                FacturaId = detalle.FacturaId,
                Descripcion = detalle.Descripcion,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Subtotal
            };

            return _detalleFacturaRepository.Insertar(entidad);
        }

        public void Actualizar(DetalleFacturaDto detalle)
        {
            var entidad = new DetalleFactura
            {
                DetalleFacturaId = detalle.DetalleFacturaId,
                FacturaId = detalle.FacturaId,
                Descripcion = detalle.Descripcion,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Subtotal
            };

            _detalleFacturaRepository.Actualizar(entidad);
        }

        public void Eliminar(int detalleFacturaId)
        {
            _detalleFacturaRepository.Eliminar(detalleFacturaId);
        }
    }
}
