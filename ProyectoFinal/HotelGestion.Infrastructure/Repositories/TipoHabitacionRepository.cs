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
    public class TipoHabitacionRepository : ITipoHabitacionRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public TipoHabitacionRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<TipoHabitacion> ObtenerTodos()
        {
            var tiposHabitacion = new List<TipoHabitacion>();

            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            const string sql = """
        SELECT
            TipoHabitacionId,
            Nombre,
            Descripcion,
            Capacidad,
            PrecioPorNoche
        FROM TipoHabitacion;
        """;

            using var command = new SqlCommand(sql, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var tipoHabitacion = new TipoHabitacion
                {
                    TipoHabitacionId = reader.GetInt32(
                        reader.GetOrdinal("TipoHabitacionId")),

                    Nombre = reader.GetString(
                        reader.GetOrdinal("Nombre")),

                    Descripcion = reader.IsDBNull(
                        reader.GetOrdinal("Descripcion"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Descripcion")),

                    Capacidad = reader.GetInt32(
                        reader.GetOrdinal("Capacidad")),

                    PrecioPorNoche = reader.GetDecimal(
                        reader.GetOrdinal("PrecioPorNoche"))
                };

                tiposHabitacion.Add(tipoHabitacion);
            }

            return tiposHabitacion;
        }

        public TipoHabitacion? ObtenerPorId(int tipoHabitacionId)
        {
            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            const string sql = """
        SELECT
            TipoHabitacionId,
            Nombre,
            Descripcion,
            Capacidad,
            PrecioPorNoche
        FROM TipoHabitacion
        WHERE TipoHabitacionId = @TipoHabitacionId;
        """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@TipoHabitacionId", SqlDbType.Int)
                .Value = tipoHabitacionId;

            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new TipoHabitacion
            {
                TipoHabitacionId = reader.GetInt32(
                    reader.GetOrdinal("TipoHabitacionId")),

                Nombre = reader.GetString(
                    reader.GetOrdinal("Nombre")),

                Descripcion = reader.IsDBNull(
                    reader.GetOrdinal("Descripcion"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Descripcion")),

                Capacidad = reader.GetInt32(
                    reader.GetOrdinal("Capacidad")),

                PrecioPorNoche = reader.GetDecimal(
                    reader.GetOrdinal("PrecioPorNoche"))
            };
        }

        public int Insertar(TipoHabitacion tipoHabitacion)
        {
            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            const string sql = """
        INSERT INTO TipoHabitacion
        (
            Nombre,
            Descripcion,
            Capacidad,
            PrecioPorNoche
        )
        OUTPUT INSERTED.TipoHabitacionId
        VALUES
        (
            @Nombre,
            @Descripcion,
            @Capacidad,
            @PrecioPorNoche
        );
        """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Nombre", SqlDbType.VarChar, 50)
                .Value = tipoHabitacion.Nombre;

            command.Parameters.Add("@Descripcion", SqlDbType.VarChar, 250)
                .Value = (object?)tipoHabitacion.Descripcion ?? DBNull.Value;

            command.Parameters.Add("@Capacidad", SqlDbType.Int)
                .Value = tipoHabitacion.Capacidad;

            command.Parameters.Add("@PrecioPorNoche", SqlDbType.Decimal)
                .Precision = 10;

            command.Parameters["@PrecioPorNoche"].Scale = 2;
            command.Parameters["@PrecioPorNoche"].Value = tipoHabitacion.PrecioPorNoche;

            return (int)command.ExecuteScalar()!;
        }

        public void Actualizar(TipoHabitacion tipoHabitacion)
        {
            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            const string sql = """
        UPDATE TipoHabitacion
        SET
            Nombre = @Nombre,
            Descripcion = @Descripcion,
            Capacidad = @Capacidad,
            PrecioPorNoche = @PrecioPorNoche
        WHERE TipoHabitacionId = @TipoHabitacionId;
        """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Nombre", SqlDbType.VarChar, 50)
                .Value = tipoHabitacion.Nombre;

            command.Parameters.Add("@Descripcion", SqlDbType.VarChar, 250)
                .Value = (object?)tipoHabitacion.Descripcion ?? DBNull.Value;

            command.Parameters.Add("@Capacidad", SqlDbType.Int)
                .Value = tipoHabitacion.Capacidad;

            var precioParameter = command.Parameters.Add(
                "@PrecioPorNoche",
                SqlDbType.Decimal);

            precioParameter.Precision = 10;
            precioParameter.Scale = 2;
            precioParameter.Value = tipoHabitacion.PrecioPorNoche;

            command.Parameters.Add("@TipoHabitacionId", SqlDbType.Int)
                .Value = tipoHabitacion.TipoHabitacionId;

            command.ExecuteNonQuery();
        }

        public void Eliminar(int tipoHabitacionId)
        {
            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            const string sql = """
        DELETE FROM TipoHabitacion
        WHERE TipoHabitacionId = @TipoHabitacionId;
        """;

            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@TipoHabitacionId", SqlDbType.Int)
                .Value = tipoHabitacionId;

            command.ExecuteNonQuery();
        }
    }
}
