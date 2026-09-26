using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IProductoMinibarService
    {
        List<ProductoMinibarDto> ObtenerTodos();

        ProductoMinibarDto? ObtenerPorId(int productoMinibarId);

        int Registrar(ProductoMinibarDto producto);

        void Actualizar(ProductoMinibarDto producto);

        void Eliminar(int productoMinibarId);
    }
}
