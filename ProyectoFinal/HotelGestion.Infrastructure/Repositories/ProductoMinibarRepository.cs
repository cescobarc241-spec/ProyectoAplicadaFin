using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using HotelGestion.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HotelGestion.Infrastructure.Repositories
{
    public class ProductoMinibarRepository : IProductoMinibarRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ProductoMinibarRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<ProductoMinibar> ObtenerTodos()
        {
            var productos = new List<ProductoMinibar>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                ProductoMinibarId,
                Nombre,
                Descripcion,
                Precio,
                Stock,
                Estado
            FROM ProductoMinibar;
            """;

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var producto = new ProductoMinibar
                {
                    ProductoMinibarId = reader.GetInt32(
                        reader.GetOrdinal("ProductoMinibarId")),

                    Nombre = reader.GetString(
                        reader.GetOrdinal("Nombre")),

                    Descripcion = reader.IsDBNull(
                        reader.GetOrdinal("Descripcion"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("Descripcion")),

                    Precio = reader.GetDecimal(
                        reader.GetOrdinal("Precio")),

                    Stock = reader.GetInt32(
                        reader.GetOrdinal("Stock")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("Estado"))
                };

                productos.Add(producto);
            }

            return productos;
        }

        public ProductoMinibar? ObtenerPorId(int productoMinibarId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                ProductoMinibarId,
                Nombre,
                Descripcion,
                Precio,
                Stock,
                Estado
            FROM ProductoMinibar
            WHERE ProductoMinibarId = @ProductoMinibarId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ProductoMinibarId",
                SqlDbType.Int).Value = productoMinibarId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new ProductoMinibar
            {
                ProductoMinibarId = reader.GetInt32(
                    reader.GetOrdinal("ProductoMinibarId")),

                Nombre = reader.GetString(
                    reader.GetOrdinal("Nombre")),

                Descripcion = reader.IsDBNull(
                    reader.GetOrdinal("Descripcion"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Descripcion")),

                Precio = reader.GetDecimal(
                    reader.GetOrdinal("Precio")),

                Stock = reader.GetInt32(
                    reader.GetOrdinal("Stock")),

                Estado = reader.GetString(
                    reader.GetOrdinal("Estado"))
            };
        }

        public int Insertar(ProductoMinibar producto)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            INSERT INTO ProductoMinibar
            (
                Nombre,
                Descripcion,
                Precio,
                Stock,
                Estado
            )
            OUTPUT INSERTED.ProductoMinibarId
            VALUES
            (
                @Nombre,
                @Descripcion,
                @Precio,
                @Stock,
                @Estado
            );
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@Nombre",
                SqlDbType.VarChar,
                100).Value = producto.Nombre;

            command.Parameters.Add(
                "@Descripcion",
                SqlDbType.VarChar,
                250).Value =
                (object?)producto.Descripcion ?? DBNull.Value;

            var precioParameter =
                command.Parameters.Add(
                    "@Precio",
                    SqlDbType.Decimal);

            precioParameter.Precision = 10;
            precioParameter.Scale = 2;
            precioParameter.Value = producto.Precio;

            command.Parameters.Add(
                "@Stock",
                SqlDbType.Int).Value = producto.Stock;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = producto.Estado;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(ProductoMinibar producto)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            UPDATE ProductoMinibar
            SET
                Nombre = @Nombre,
                Descripcion = @Descripcion,
                Precio = @Precio,
                Stock = @Stock,
                Estado = @Estado
            WHERE ProductoMinibarId = @ProductoMinibarId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@Nombre",
                SqlDbType.VarChar,
                100).Value = producto.Nombre;

            command.Parameters.Add(
                "@Descripcion",
                SqlDbType.VarChar,
                250).Value =
                (object?)producto.Descripcion ?? DBNull.Value;

            var precioParameter =
                command.Parameters.Add(
                    "@Precio",
                    SqlDbType.Decimal);

            precioParameter.Precision = 10;
            precioParameter.Scale = 2;
            precioParameter.Value = producto.Precio;

            command.Parameters.Add(
                "@Stock",
                SqlDbType.Int).Value = producto.Stock;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = producto.Estado;

            command.Parameters.Add(
                "@ProductoMinibarId",
                SqlDbType.Int).Value = producto.ProductoMinibarId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int productoMinibarId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            DELETE FROM ProductoMinibar
            WHERE ProductoMinibarId = @ProductoMinibarId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ProductoMinibarId",
                SqlDbType.Int).Value = productoMinibarId;

            command.ExecuteNonQuery();
        }
    }
}
