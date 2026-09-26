using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface IDetalleFacturaRepository
    {
        List<DetalleFactura> ObtenerTodos();

        DetalleFactura? ObtenerPorId(int detalleFacturaId);

        int Insertar(DetalleFactura detalleFactura);

        void Actualizar(DetalleFactura detalleFactura);

        void Eliminar(int detalleFacturaId);
    }
}
