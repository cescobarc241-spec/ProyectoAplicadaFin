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
    public class ReservaRepository : IReservaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public ReservaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<Reserva> ObtenerTodos()
        {
            var reservas = new List<Reserva>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                ReservaId,
                ClienteId,
                HabitacionId,
                FechaReserva,
                FechaEntrada,
                FechaSalida,
                CantidadHuespedes,
                Estado,
                Observaciones
            FROM Reserva;
            """;

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var reserva = new Reserva
                {
                    ReservaId = reader.GetInt32(
                        reader.GetOrdinal("ReservaId")),

                    ClienteId = reader.GetInt32(
                        reader.GetOrdinal("ClienteId")),

                    HabitacionId = reader.GetInt32(
                        reader.GetOrdinal("HabitacionId")),

                    FechaReserva = reader.GetDateTime(
                        reader.GetOrdinal("FechaReserva")),

                    FechaEntrada = reader.GetDateTime(
                        reader.GetOrdinal("FechaEntrada")),

                    FechaSalida = reader.GetDateTime(
                        reader.GetOrdinal("FechaSalida")),

                    CantidadHuespedes = reader.GetInt32(
                        reader.GetOrdinal("CantidadHuespedes")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("Estado")),

                    Observaciones = reader.IsDBNull(
                        reader.GetOrdinal("Observaciones"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("Observaciones"))
                };

                reservas.Add(reserva);
            }

            return reservas;
        }

        public Reserva? ObtenerPorId(int reservaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                ReservaId,
                ClienteId,
                HabitacionId,
                FechaReserva,
                FechaEntrada,
                FechaSalida,
                CantidadHuespedes,
                Estado,
                Observaciones
            FROM Reserva
            WHERE ReservaId = @ReservaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ReservaId",
                SqlDbType.Int).Value = reservaId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new Reserva
            {
                ReservaId = reader.GetInt32(
                    reader.GetOrdinal("ReservaId")),

                ClienteId = reader.GetInt32(
                    reader.GetOrdinal("ClienteId")),

                HabitacionId = reader.GetInt32(
                    reader.GetOrdinal("HabitacionId")),

                FechaReserva = reader.GetDateTime(
                    reader.GetOrdinal("FechaReserva")),

                FechaEntrada = reader.GetDateTime(
                    reader.GetOrdinal("FechaEntrada")),

                FechaSalida = reader.GetDateTime(
                    reader.GetOrdinal("FechaSalida")),

                CantidadHuespedes = reader.GetInt32(
                    reader.GetOrdinal("CantidadHuespedes")),

                Estado = reader.GetString(
                    reader.GetOrdinal("Estado")),

                Observaciones = reader.IsDBNull(
                    reader.GetOrdinal("Observaciones"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Observaciones"))
            };
        }

        public int Insertar(Reserva reserva)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            INSERT INTO Reserva
            (
                ClienteId,
                HabitacionId,
                FechaReserva,
                FechaEntrada,
                FechaSalida,
                CantidadHuespedes,
                Estado,
                Observaciones
            )
            OUTPUT INSERTED.ReservaId
            VALUES
            (
                @ClienteId,
                @HabitacionId,
                @FechaReserva,
                @FechaEntrada,
                @FechaSalida,
                @CantidadHuespedes,
                @Estado,
                @Observaciones
            );
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ClienteId",
                SqlDbType.Int).Value = reserva.ClienteId;

            command.Parameters.Add(
                "@HabitacionId",
                SqlDbType.Int).Value = reserva.HabitacionId;

            command.Parameters.Add(
                "@FechaReserva",
                SqlDbType.DateTime2).Value = reserva.FechaReserva;

            command.Parameters.Add(
                "@FechaEntrada",
                SqlDbType.DateTime2).Value = reserva.FechaEntrada;

            command.Parameters.Add(
                "@FechaSalida",
                SqlDbType.DateTime2).Value = reserva.FechaSalida;

            command.Parameters.Add(
                "@CantidadHuespedes",
                SqlDbType.Int).Value = reserva.CantidadHuespedes;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = reserva.Estado;

            command.Parameters.Add(
                "@Observaciones",
                SqlDbType.VarChar,
                500).Value =
                (object?)reserva.Observaciones ?? DBNull.Value;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(Reserva reserva)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            UPDATE Reserva
            SET
                ClienteId = @ClienteId,
                HabitacionId = @HabitacionId,
                FechaReserva = @FechaReserva,
                FechaEntrada = @FechaEntrada,
                FechaSalida = @FechaSalida,
                CantidadHuespedes = @CantidadHuespedes,
                Estado = @Estado,
                Observaciones = @Observaciones
            WHERE ReservaId = @ReservaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ClienteId",
                SqlDbType.Int).Value = reserva.ClienteId;

            command.Parameters.Add(
                "@HabitacionId",
                SqlDbType.Int).Value = reserva.HabitacionId;

            command.Parameters.Add(
                "@FechaReserva",
                SqlDbType.DateTime2).Value = reserva.FechaReserva;

            command.Parameters.Add(
                "@FechaEntrada",
                SqlDbType.DateTime2).Value = reserva.FechaEntrada;

            command.Parameters.Add(
                "@FechaSalida",
                SqlDbType.DateTime2).Value = reserva.FechaSalida;

            command.Parameters.Add(
                "@CantidadHuespedes",
                SqlDbType.Int).Value = reserva.CantidadHuespedes;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = reserva.Estado;

            command.Parameters.Add(
                "@Observaciones",
                SqlDbType.VarChar,
                500).Value =
                (object?)reserva.Observaciones ?? DBNull.Value;

            command.Parameters.Add(
                "@ReservaId",
                SqlDbType.Int).Value = reserva.ReservaId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int reservaId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            DELETE FROM Reserva
            WHERE ReservaId = @ReservaId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@ReservaId",
                SqlDbType.Int).Value = reservaId;

            command.ExecuteNonQuery();
        }
    }
}
