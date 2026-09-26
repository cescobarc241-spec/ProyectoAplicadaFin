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
    public class EstanciaRepository : IEstanciaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public EstanciaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<Estancia> ObtenerTodos()
        {
            var estancias = new List<Estancia>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                EstanciaId,
                ReservaId,
                FechaCheckIn,
                FechaCheckOut,
                Estado,
                Observaciones
            FROM Estancia;
            """;

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var estancia = new Estancia
                {
                    EstanciaId = reader.GetInt32(
                        reader.GetOrdinal("EstanciaId")),

                    ReservaId = reader.GetInt32(
                        reader.GetOrdinal("ReservaId")),

                    FechaCheckIn = reader.GetDateTime(
                        reader.GetOrdinal("FechaCheckIn")),

                    FechaCheckOut = reader.IsDBNull(
                        reader.GetOrdinal("FechaCheckOut"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("FechaCheckOut")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("Estado")),

                    Observaciones = reader.IsDBNull(
                        reader.GetOrdinal("Observaciones"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("Observaciones"))
                };

                estancias.Add(estancia);
            }

            return estancias;
        }

        public Estancia? ObtenerPorId(int estanciaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                EstanciaId,
                ReservaId,
                FechaCheckIn,
                FechaCheckOut,
                Estado,
                Observaciones
            FROM Estancia
            WHERE EstanciaId = @EstanciaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@EstanciaId",
                SqlDbType.Int).Value = estanciaId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new Estancia
            {
                EstanciaId = reader.GetInt32(
                    reader.GetOrdinal("EstanciaId")),

                ReservaId = reader.GetInt32(
                    reader.GetOrdinal("ReservaId")),

                FechaCheckIn = reader.GetDateTime(
                    reader.GetOrdinal("FechaCheckIn")),

                FechaCheckOut = reader.IsDBNull(
                    reader.GetOrdinal("FechaCheckOut"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("FechaCheckOut")),

                Estado = reader.GetString(
                    reader.GetOrdinal("Estado")),

                Observaciones = reader.IsDBNull(
                    reader.GetOrdinal("Observaciones"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Observaciones"))
            };
        }

        public int Insertar(Estancia estancia)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            INSERT INTO Estancia
            (
                ReservaId,
                FechaCheckIn,
                FechaCheckOut,
                Estado,
                Observaciones
            )
            OUTPUT INSERTED.EstanciaId
            VALUES
            (
                @ReservaId,
                @FechaCheckIn,
                @FechaCheckOut,
                @Estado,
                @Observaciones
            );
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ReservaId",
                SqlDbType.Int).Value = estancia.ReservaId;

            command.Parameters.Add(
                "@FechaCheckIn",
                SqlDbType.DateTime2).Value = estancia.FechaCheckIn;

            command.Parameters.Add(
                "@FechaCheckOut",
                SqlDbType.DateTime2).Value =
                (object?)estancia.FechaCheckOut ?? DBNull.Value;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = estancia.Estado;

            command.Parameters.Add(
                "@Observaciones",
                SqlDbType.VarChar,
                500).Value =
                (object?)estancia.Observaciones ?? DBNull.Value;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(Estancia estancia)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            UPDATE Estancia
            SET
                ReservaId = @ReservaId,
                FechaCheckIn = @FechaCheckIn,
                FechaCheckOut = @FechaCheckOut,
                Estado = @Estado,
                Observaciones = @Observaciones
            WHERE EstanciaId = @EstanciaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ReservaId",
                SqlDbType.Int).Value = estancia.ReservaId;

            command.Parameters.Add(
                "@FechaCheckIn",
                SqlDbType.DateTime2).Value = estancia.FechaCheckIn;

            command.Parameters.Add(
                "@FechaCheckOut",
                SqlDbType.DateTime2).Value =
                (object?)estancia.FechaCheckOut ?? DBNull.Value;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = estancia.Estado;

            command.Parameters.Add(
                "@Observaciones",
                SqlDbType.VarChar,
                500).Value =
                (object?)estancia.Observaciones ?? DBNull.Value;

            command.Parameters.Add(
                "@EstanciaId",
                SqlDbType.Int).Value = estancia.EstanciaId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int estanciaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            DELETE FROM Estancia
            WHERE EstanciaId = @EstanciaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@EstanciaId",
                SqlDbType.Int).Value = estanciaId;

            command.ExecuteNonQuery();
        }
    }
}
