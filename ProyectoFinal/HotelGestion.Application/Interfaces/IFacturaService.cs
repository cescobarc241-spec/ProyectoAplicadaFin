using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IFacturaService
    {
        List<FacturaDto> ObtenerTodos();

        FacturaDto? ObtenerPorId(int facturaId);

        int Registrar(FacturaDto factura);

        void Actualizar(FacturaDto factura);

        void Eliminar(int facturaId);
    }
}
