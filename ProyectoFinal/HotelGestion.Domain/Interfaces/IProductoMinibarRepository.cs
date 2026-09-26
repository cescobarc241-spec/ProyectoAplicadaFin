using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface IProductoMinibarRepository
    {
        List<ProductoMinibar> ObtenerTodos();

        ProductoMinibar? ObtenerPorId(int productoMinibarId);

        int Insertar(ProductoMinibar productoMinibar);

        void Actualizar(ProductoMinibar productoMinibar);

        void Eliminar(int productoMinibarId);
    }
}
