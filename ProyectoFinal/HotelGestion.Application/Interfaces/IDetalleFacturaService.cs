using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IDetalleFacturaService
    {
        List<DetalleFacturaDto> ObtenerTodos();

        DetalleFacturaDto? ObtenerPorId(int detalleFacturaId);

        int Registrar(DetalleFacturaDto detalle);

        void Actualizar(DetalleFacturaDto detalle);

        void Eliminar(int detalleFacturaId);
    }
}
