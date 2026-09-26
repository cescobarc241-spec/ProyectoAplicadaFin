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
    public class ConsumoMinibarRepository : IConsumoMinibarRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ConsumoMinibarRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<ConsumoMinibar> ObtenerTodos()
        {
            var consumos = new List<ConsumoMinibar>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                ConsumoMinibarId,
                EstanciaId,
                ProductoMinibarId,
                Cantidad,
                PrecioUnitario,
                FechaConsumo
            FROM ConsumoMinibar;
            """;

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var consumo = new ConsumoMinibar
                {
                    ConsumoMinibarId = reader.GetInt32(
                        reader.GetOrdinal("ConsumoMinibarId")),

                    EstanciaId = reader.GetInt32(
                        reader.GetOrdinal("EstanciaId")),

                    ProductoMinibarId = reader.GetInt32(
                        reader.GetOrdinal("ProductoMinibarId")),

                    Cantidad = reader.GetInt32(
                        reader.GetOrdinal("Cantidad")),

                    PrecioUnitario = reader.GetDecimal(
                        reader.GetOrdinal("PrecioUnitario")),

                    FechaConsumo = reader.GetDateTime(
                        reader.GetOrdinal("FechaConsumo"))
                };

                consumos.Add(consumo);
            }

            return consumos;
        }

        public ConsumoMinibar? ObtenerPorId(int consumoMinibarId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                ConsumoMinibarId,
                EstanciaId,
                ProductoMinibarId,
                Cantidad,
                PrecioUnitario,
                FechaConsumo
            FROM ConsumoMinibar
            WHERE ConsumoMinibarId = @ConsumoMinibarId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ConsumoMinibarId",
                SqlDbType.Int).Value = consumoMinibarId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new ConsumoMinibar
            {
                ConsumoMinibarId = reader.GetInt32(
                    reader.GetOrdinal("ConsumoMinibarId")),

                EstanciaId = reader.GetInt32(
                    reader.GetOrdinal("EstanciaId")),

                ProductoMinibarId = reader.GetInt32(
                    reader.GetOrdinal("ProductoMinibarId")),

                Cantidad = reader.GetInt32(
                    reader.GetOrdinal("Cantidad")),

                PrecioUnitario = reader.GetDecimal(
                    reader.GetOrdinal("PrecioUnitario")),

                FechaConsumo = reader.GetDateTime(
                    reader.GetOrdinal("FechaConsumo"))
            };
        }

        public int Insertar(ConsumoMinibar consumo)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            INSERT INTO ConsumoMinibar
            (
                EstanciaId,
                ProductoMinibarId,
                Cantidad,
                PrecioUnitario,
                FechaConsumo
            )
            OUTPUT INSERTED.ConsumoMinibarId
            VALUES
            (
                @EstanciaId,
                @ProductoMinibarId,
                @Cantidad,
                @PrecioUnitario,
                @FechaConsumo
            );
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@EstanciaId",
                SqlDbType.Int).Value = consumo.EstanciaId;

            command.Parameters.Add(
                "@ProductoMinibarId",
                SqlDbType.Int).Value = consumo.ProductoMinibarId;

            command.Parameters.Add(
                "@Cantidad",
                SqlDbType.Int).Value = consumo.Cantidad;

            var precioParameter =
                command.Parameters.Add(
                    "@PrecioUnitario",
                    SqlDbType.Decimal);

            precioParameter.Precision = 10;
            precioParameter.Scale = 2;
            precioParameter.Value = consumo.PrecioUnitario;

            command.Parameters.Add(
                "@FechaConsumo",
                SqlDbType.DateTime2).Value = consumo.FechaConsumo;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(ConsumoMinibar consumo)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            UPDATE ConsumoMinibar
            SET
                EstanciaId = @EstanciaId,
                ProductoMinibarId = @ProductoMinibarId,
                Cantidad = @Cantidad,
                PrecioUnitario = @PrecioUnitario,
                FechaConsumo = @FechaConsumo
            WHERE ConsumoMinibarId = @ConsumoMinibarId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@EstanciaId",
                SqlDbType.Int).Value = consumo.EstanciaId;

            command.Parameters.Add(
                "@ProductoMinibarId",
                SqlDbType.Int).Value = consumo.ProductoMinibarId;

            command.Parameters.Add(
                "@Cantidad",
                SqlDbType.Int).Value = consumo.Cantidad;

            var precioParameter =
                command.Parameters.Add(
                    "@PrecioUnitario",
                    SqlDbType.Decimal);

            precioParameter.Precision = 10;
            precioParameter.Scale = 2;
            precioParameter.Value = consumo.PrecioUnitario;

            command.Parameters.Add(
                "@FechaConsumo",
                SqlDbType.DateTime2).Value = consumo.FechaConsumo;

            command.Parameters.Add(
                "@ConsumoMinibarId",
                SqlDbType.Int).Value = consumo.ConsumoMinibarId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int consumoMinibarId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            DELETE FROM ConsumoMinibar
            WHERE ConsumoMinibarId = @ConsumoMinibarId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ConsumoMinibarId",
                SqlDbType.Int).Value = consumoMinibarId;

            command.ExecuteNonQuery();
        }
    }
}
