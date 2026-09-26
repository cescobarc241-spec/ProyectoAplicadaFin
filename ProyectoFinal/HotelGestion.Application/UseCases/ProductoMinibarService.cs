using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.UseCases
{
    public class ProductoMinibarService : IProductoMinibarService
    {
        private readonly IProductoMinibarRepository _productoMinibarRepository;

        public ProductoMinibarService(
            IProductoMinibarRepository productoMinibarRepository)
        {
            _productoMinibarRepository = productoMinibarRepository;
        }

        public List<ProductoMinibarDto> ObtenerTodos()
        {
            var productos = _productoMinibarRepository.ObtenerTodos();

            return productos.Select(p => new ProductoMinibarDto
            {
                ProductoMinibarId = p.ProductoMinibarId,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                Estado = p.Estado
            }).ToList();
        }

        public ProductoMinibarDto? ObtenerPorId(int productoMinibarId)
        {
            var producto =
                _productoMinibarRepository.ObtenerPorId(productoMinibarId);

            if (producto == null)
                return null;

            return new ProductoMinibarDto
            {
                ProductoMinibarId = producto.ProductoMinibarId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Estado = producto.Estado
            };
        }

        public int Registrar(ProductoMinibarDto producto)
        {
            var entidad = new ProductoMinibar
            {
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Estado = producto.Estado
            };

            return _productoMinibarRepository.Insertar(entidad);
        }

        public void Actualizar(ProductoMinibarDto producto)
        {
            var entidad = new ProductoMinibar
            {
                ProductoMinibarId = producto.ProductoMinibarId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Estado = producto.Estado
            };

            _productoMinibarRepository.Actualizar(entidad);
        }

        public void Eliminar(int productoMinibarId)
        {
            _productoMinibarRepository.Eliminar(productoMinibarId);
        }
    }
}
