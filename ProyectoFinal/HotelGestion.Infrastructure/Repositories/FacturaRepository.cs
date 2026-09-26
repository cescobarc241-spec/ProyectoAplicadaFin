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
    public class FacturaRepository : IFacturaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public FacturaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<Factura> ObtenerTodos()
        {
            var facturas = new List<Factura>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                FacturaId,
                EstanciaId,
                NumeroFactura,
                FechaEmision,
                Subtotal,
                Impuesto,
                Total,
                MetodoPago,
                Estado
            FROM Factura;
            """;

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var factura = new Factura
                {
                    FacturaId = reader.GetInt32(
                        reader.GetOrdinal("FacturaId")),

                    EstanciaId = reader.GetInt32(
                        reader.GetOrdinal("EstanciaId")),

                    NumeroFactura = reader.GetString(
                        reader.GetOrdinal("NumeroFactura")),

                    FechaEmision = reader.GetDateTime(
                        reader.GetOrdinal("FechaEmision")),

                    Subtotal = reader.GetDecimal(
                        reader.GetOrdinal("Subtotal")),

                    Impuesto = reader.GetDecimal(
                        reader.GetOrdinal("Impuesto")),

                    Total = reader.GetDecimal(
                        reader.GetOrdinal("Total")),

                    MetodoPago = reader.GetString(
                        reader.GetOrdinal("MetodoPago")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("Estado"))
                };

                facturas.Add(factura);
            }

            return facturas;
        }

        public Factura? ObtenerPorId(int facturaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                FacturaId,
                EstanciaId,
                NumeroFactura,
                FechaEmision,
                Subtotal,
                Impuesto,
                Total,
                MetodoPago,
                Estado
            FROM Factura
            WHERE FacturaId = @FacturaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@FacturaId",
                SqlDbType.Int).Value = facturaId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new Factura
            {
                FacturaId = reader.GetInt32(
                    reader.GetOrdinal("FacturaId")),

                EstanciaId = reader.GetInt32(
                    reader.GetOrdinal("EstanciaId")),

                NumeroFactura = reader.GetString(
                    reader.GetOrdinal("NumeroFactura")),

                FechaEmision = reader.GetDateTime(
                    reader.GetOrdinal("FechaEmision")),

                Subtotal = reader.GetDecimal(
                    reader.GetOrdinal("Subtotal")),

                Impuesto = reader.GetDecimal(
                    reader.GetOrdinal("Impuesto")),

                Total = reader.GetDecimal(
                    reader.GetOrdinal("Total")),

                MetodoPago = reader.GetString(
                    reader.GetOrdinal("MetodoPago")),

                Estado = reader.GetString(
                    reader.GetOrdinal("Estado"))
            };
        }

        public int Insertar(Factura factura)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            INSERT INTO Factura
            (
                EstanciaId,
                NumeroFactura,
                FechaEmision,
                Subtotal,
                Impuesto,
                Total,
                MetodoPago,
                Estado
            )
            OUTPUT INSERTED.FacturaId
            VALUES
            (
                @EstanciaId,
                @NumeroFactura,
                @FechaEmision,
                @Subtotal,
                @Impuesto,
                @Total,
                @MetodoPago,
                @Estado
            );
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@EstanciaId",
                SqlDbType.Int).Value = factura.EstanciaId;

            command.Parameters.Add(
                "@NumeroFactura",
                SqlDbType.VarChar,
                30).Value = factura.NumeroFactura;

            command.Parameters.Add(
                "@FechaEmision",
                SqlDbType.DateTime2).Value = factura.FechaEmision;

            var subtotalParameter =
                command.Parameters.Add(
                    "@Subtotal",
                    SqlDbType.Decimal);

            subtotalParameter.Precision = 12;
            subtotalParameter.Scale = 2;
            subtotalParameter.Value = factura.Subtotal;

            var impuestoParameter =
                command.Parameters.Add(
                    "@Impuesto",
                    SqlDbType.Decimal);

            impuestoParameter.Precision = 12;
            impuestoParameter.Scale = 2;
            impuestoParameter.Value = factura.Impuesto;

            var totalParameter =
                command.Parameters.Add(
                    "@Total",
                    SqlDbType.Decimal);

            totalParameter.Precision = 12;
            totalParameter.Scale = 2;
            totalParameter.Value = factura.Total;

            command.Parameters.Add(
                "@MetodoPago",
                SqlDbType.VarChar,
                20).Value = factura.MetodoPago;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = factura.Estado;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(Factura factura)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            UPDATE Factura
            SET
                EstanciaId = @EstanciaId,
                NumeroFactura = @NumeroFactura,
                FechaEmision = @FechaEmision,
                Subtotal = @Subtotal,
                Impuesto = @Impuesto,
                Total = @Total,
                MetodoPago = @MetodoPago,
                Estado = @Estado
            WHERE FacturaId = @FacturaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@EstanciaId",
                SqlDbType.Int).Value = factura.EstanciaId;

            command.Parameters.Add(
                "@NumeroFactura",
                SqlDbType.VarChar,
                30).Value = factura.NumeroFactura;

            command.Parameters.Add(
                "@FechaEmision",
                SqlDbType.DateTime2).Value = factura.FechaEmision;

            var subtotalParameter =
                command.Parameters.Add(
                    "@Subtotal",
                    SqlDbType.Decimal);

            subtotalParameter.Precision = 12;
            subtotalParameter.Scale = 2;
            subtotalParameter.Value = factura.Subtotal;

            var impuestoParameter =
                command.Parameters.Add(
                    "@Impuesto",
                    SqlDbType.Decimal);

            impuestoParameter.Precision = 12;
            impuestoParameter.Scale = 2;
            impuestoParameter.Value = factura.Impuesto;

            var totalParameter =
                command.Parameters.Add(
                    "@Total",
                    SqlDbType.Decimal);

            totalParameter.Precision = 12;
            totalParameter.Scale = 2;
            totalParameter.Value = factura.Total;

            command.Parameters.Add(
                "@MetodoPago",
                SqlDbType.VarChar,
                20).Value = factura.MetodoPago;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = factura.Estado;

            command.Parameters.Add(
                "@FacturaId",
                SqlDbType.Int).Value = factura.FacturaId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int facturaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            DELETE FROM Factura
            WHERE FacturaId = @FacturaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@FacturaId",
                SqlDbType.Int).Value = facturaId;

            command.ExecuteNonQuery();
        }
    }
}
