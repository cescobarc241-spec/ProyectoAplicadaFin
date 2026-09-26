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
    public class DetalleFacturaRepository : IDetalleFacturaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public DetalleFacturaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<DetalleFactura> ObtenerTodos()
        {
            var detalles = new List<DetalleFactura>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                DetalleFacturaId,
                FacturaId,
                Descripcion,
                Cantidad,
                PrecioUnitario,
                Subtotal
            FROM DetalleFactura;
            """;

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var detalle = new DetalleFactura
                {
                    DetalleFacturaId = reader.GetInt32(
                        reader.GetOrdinal("DetalleFacturaId")),

                    FacturaId = reader.GetInt32(
                        reader.GetOrdinal("FacturaId")),

                    Descripcion = reader.GetString(
                        reader.GetOrdinal("Descripcion")),

                    Cantidad = reader.GetInt32(
                        reader.GetOrdinal("Cantidad")),

                    PrecioUnitario = reader.GetDecimal(
                        reader.GetOrdinal("PrecioUnitario")),

                    Subtotal = reader.GetDecimal(
                        reader.GetOrdinal("Subtotal"))
                };

                detalles.Add(detalle);
            }

            return detalles;
        }

        public DetalleFactura? ObtenerPorId(int detalleFacturaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                DetalleFacturaId,
                FacturaId,
                Descripcion,
                Cantidad,
                PrecioUnitario,
                Subtotal
            FROM DetalleFactura
            WHERE DetalleFacturaId = @DetalleFacturaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@DetalleFacturaId",
                SqlDbType.Int).Value = detalleFacturaId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new DetalleFactura
            {
                DetalleFacturaId = reader.GetInt32(
                    reader.GetOrdinal("DetalleFacturaId")),

                FacturaId = reader.GetInt32(
                    reader.GetOrdinal("FacturaId")),

                Descripcion = reader.GetString(
                    reader.GetOrdinal("Descripcion")),

                Cantidad = reader.GetInt32(
                    reader.GetOrdinal("Cantidad")),

                PrecioUnitario = reader.GetDecimal(
                    reader.GetOrdinal("PrecioUnitario")),

                Subtotal = reader.GetDecimal(
                    reader.GetOrdinal("Subtotal"))
            };
        }

        public int Insertar(DetalleFactura detalle)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            INSERT INTO DetalleFactura
            (
                FacturaId,
                Descripcion,
                Cantidad,
                PrecioUnitario,
                Subtotal
            )
            OUTPUT INSERTED.DetalleFacturaId
            VALUES
            (
                @FacturaId,
                @Descripcion,
                @Cantidad,
                @PrecioUnitario,
                @Subtotal
            );
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@FacturaId",
                SqlDbType.Int).Value = detalle.FacturaId;

            command.Parameters.Add(
                "@Descripcion",
                SqlDbType.VarChar,
                250).Value = detalle.Descripcion;

            command.Parameters.Add(
                "@Cantidad",
                SqlDbType.Int).Value = detalle.Cantidad;

            var precioParameter =
                command.Parameters.Add("@PrecioUnitario", SqlDbType.Decimal);

            precioParameter.Precision = 12;
            precioParameter.Scale = 2;
            precioParameter.Value = detalle.PrecioUnitario;

            var subtotalParameter =
                command.Parameters.Add("@Subtotal", SqlDbType.Decimal);

            subtotalParameter.Precision = 12;
            subtotalParameter.Scale = 2;
            subtotalParameter.Value = detalle.Subtotal;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(DetalleFactura detalle)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            UPDATE DetalleFactura
            SET
                FacturaId = @FacturaId,
                Descripcion = @Descripcion,
                Cantidad = @Cantidad,
                PrecioUnitario = @PrecioUnitario,
                Subtotal = @Subtotal
            WHERE DetalleFacturaId = @DetalleFacturaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@FacturaId",
                SqlDbType.Int).Value = detalle.FacturaId;

            command.Parameters.Add(
                "@Descripcion",
                SqlDbType.VarChar,
                250).Value = detalle.Descripcion;

            command.Parameters.Add(
                "@Cantidad",
                SqlDbType.Int).Value = detalle.Cantidad;

            var precioParameter =
                command.Parameters.Add("@PrecioUnitario", SqlDbType.Decimal);

            precioParameter.Precision = 12;
            precioParameter.Scale = 2;
            precioParameter.Value = detalle.PrecioUnitario;

            var subtotalParameter =
                command.Parameters.Add("@Subtotal", SqlDbType.Decimal);

            subtotalParameter.Precision = 12;
            subtotalParameter.Scale = 2;
            subtotalParameter.Value = detalle.Subtotal;

            command.Parameters.Add(
                "@DetalleFacturaId",
                SqlDbType.Int).Value = detalle.DetalleFacturaId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int detalleFacturaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            DELETE FROM DetalleFactura
            WHERE DetalleFacturaId = @DetalleFacturaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@DetalleFacturaId",
                SqlDbType.Int).Value = detalleFacturaId;

            command.ExecuteNonQuery();
        }
    }
}
