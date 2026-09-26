using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface IFacturaRepository
    {
        List<Factura> ObtenerTodos();

        Factura? ObtenerPorId(int facturaId);

        int Insertar(Factura factura);

        void Actualizar(Factura factura);

        void Eliminar(int facturaId);
    }
}
