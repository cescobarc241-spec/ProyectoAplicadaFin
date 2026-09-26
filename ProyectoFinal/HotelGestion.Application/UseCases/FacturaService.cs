using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;

        public FacturaService(IFacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
        }

        public List<FacturaDto> ObtenerTodos()
        {
            var facturas = _facturaRepository.ObtenerTodos();

            return facturas.Select(f => new FacturaDto
            {
                FacturaId = f.FacturaId,
                EstanciaId = f.EstanciaId,
                NumeroFactura = f.NumeroFactura,
                FechaEmision = f.FechaEmision,
                Subtotal = f.Subtotal,
                Impuesto = f.Impuesto,
                Total = f.Total,
                MetodoPago = f.MetodoPago,
                Estado = f.Estado
            }).ToList();
        }

        public FacturaDto? ObtenerPorId(int facturaId)
        {
            var factura = _facturaRepository.ObtenerPorId(facturaId);

            if (factura == null)
                return null;

            return new FacturaDto
            {
                FacturaId = factura.FacturaId,
                EstanciaId = factura.EstanciaId,
                NumeroFactura = factura.NumeroFactura,
                FechaEmision = factura.FechaEmision,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Total = factura.Total,
                MetodoPago = factura.MetodoPago,
                Estado = factura.Estado
            };
        }

        public int Registrar(FacturaDto factura)
        {
            var entidad = new Factura
            {
                EstanciaId = factura.EstanciaId,
                NumeroFactura = factura.NumeroFactura,
                FechaEmision = factura.FechaEmision,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Total = factura.Total,
                MetodoPago = factura.MetodoPago,
                Estado = factura.Estado
            };

            return _facturaRepository.Insertar(entidad);
        }

        public void Actualizar(FacturaDto factura)
        {
            var entidad = new Factura
            {
                FacturaId = factura.FacturaId,
                EstanciaId = factura.EstanciaId,
                NumeroFactura = factura.NumeroFactura,
                FechaEmision = factura.FechaEmision,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Total = factura.Total,
                MetodoPago = factura.MetodoPago,
                Estado = factura.Estado
            };

            _facturaRepository.Actualizar(entidad);
        }

        public void Eliminar(int facturaId)
        {
            _facturaRepository.Eliminar(facturaId);
        }
    }
}
