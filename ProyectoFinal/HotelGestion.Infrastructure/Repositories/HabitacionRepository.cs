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
    public class HabitacionRepository : IHabitacionRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public HabitacionRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<Habitacion> ObtenerTodos()
        {
            var habitaciones = new List<Habitacion>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                HabitacionId,
                Numero,
                Piso,
                TipoHabitacionId,
                Estado,
                Descripcion
            FROM Habitacion;
            """;

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var habitacion = new Habitacion
                {
                    HabitacionId = reader.GetInt32(
                        reader.GetOrdinal("HabitacionId")),

                    Numero = reader.GetString(
                        reader.GetOrdinal("Numero")),

                    Piso = reader.GetInt32(
                        reader.GetOrdinal("Piso")),

                    TipoHabitacionId = reader.GetInt32(
                        reader.GetOrdinal("TipoHabitacionId")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("Estado")),

                    Descripcion = reader.IsDBNull(
                        reader.GetOrdinal("Descripcion"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("Descripcion"))
                };

                habitaciones.Add(habitacion);
            }

            return habitaciones;
        }

        public Habitacion? ObtenerPorId(int habitacionId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            SELECT
                HabitacionId,
                Numero,
                Piso,
                TipoHabitacionId,
                Estado,
                Descripcion
            FROM Habitacion
            WHERE HabitacionId = @HabitacionId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@HabitacionId",
                SqlDbType.Int).Value = habitacionId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new Habitacion
            {
                HabitacionId = reader.GetInt32(
                    reader.GetOrdinal("HabitacionId")),

                Numero = reader.GetString(
                    reader.GetOrdinal("Numero")),

                Piso = reader.GetInt32(
                    reader.GetOrdinal("Piso")),

                TipoHabitacionId = reader.GetInt32(
                    reader.GetOrdinal("TipoHabitacionId")),

                Estado = reader.GetString(
                    reader.GetOrdinal("Estado")),

                Descripcion = reader.IsDBNull(
                    reader.GetOrdinal("Descripcion"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Descripcion"))
            };
        }

        public int Insertar(Habitacion habitacion)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            INSERT INTO Habitacion
            (
                Numero,
                Piso,
                TipoHabitacionId,
                Estado,
                Descripcion
            )
            OUTPUT INSERTED.HabitacionId
            VALUES
            (
                @Numero,
                @Piso,
                @TipoHabitacionId,
                @Estado,
                @Descripcion
            );
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@Numero",
                SqlDbType.VarChar,
                10).Value = habitacion.Numero;

            command.Parameters.Add(
                "@Piso",
                SqlDbType.Int).Value = habitacion.Piso;

            command.Parameters.Add(
                "@TipoHabitacionId",
                SqlDbType.Int).Value = habitacion.TipoHabitacionId;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = habitacion.Estado;

            command.Parameters.Add(
                "@Descripcion",
                SqlDbType.VarChar,
                250).Value =
                (object?)habitacion.Descripcion ?? DBNull.Value;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(Habitacion habitacion)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            UPDATE Habitacion
            SET
                Numero = @Numero,
                Piso = @Piso,
                TipoHabitacionId = @TipoHabitacionId,
                Estado = @Estado,
                Descripcion = @Descripcion
            WHERE HabitacionId = @HabitacionId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@Numero",
                SqlDbType.VarChar,
                10).Value = habitacion.Numero;

            command.Parameters.Add(
                "@Piso",
                SqlDbType.Int).Value = habitacion.Piso;

            command.Parameters.Add(
                "@TipoHabitacionId",
                SqlDbType.Int).Value = habitacion.TipoHabitacionId;

            command.Parameters.Add(
                "@Estado",
                SqlDbType.VarChar,
                20).Value = habitacion.Estado;

            command.Parameters.Add(
                "@Descripcion",
                SqlDbType.VarChar,
                250).Value =
                (object?)habitacion.Descripcion ?? DBNull.Value;

            command.Parameters.Add(
                "@HabitacionId",
                SqlDbType.Int).Value = habitacion.HabitacionId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int habitacionId)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            const string sql = """
            DELETE FROM Habitacion
            WHERE HabitacionId = @HabitacionId;
            """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add(
                "@HabitacionId",
                SqlDbType.Int).Value = habitacionId;

            command.ExecuteNonQuery();
        }
    }
}
