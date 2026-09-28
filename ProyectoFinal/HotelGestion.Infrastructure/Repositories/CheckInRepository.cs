using Microsoft.Data.SqlClient;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Infrastructure.Data;
using HotelGestion.Infrastructure.Transactions;
using System.Data;

namespace HotelGestion.Infrastructure.Repositories;

public class CheckInRepository : ICheckInRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public CheckInRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public int Ejecutar(CheckInDto checkIn)
    {
        using var transactionManager =
            new TransactionManager(_connectionFactory);

        try
        {
            int reservaId;
            int habitacionId;
            int clienteId;

            // =========================================================
            // 1. CHECK-IN CON RESERVA
            // =========================================================
            if (checkIn.ConReserva)
            {
                if (checkIn.ReservaId <= 0)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar una reserva.");
                }

                const string sqlReserva = """
                    SELECT
                        ReservaId,
                        ClienteId,
                        HabitacionId,
                        Estado,
                        FechaEntrada,
                        FechaSalida,
                        CantidadHuespedes
                    FROM Reserva
                    WHERE ReservaId = @ReservaId;
                    """;

                using (var command = new SqlCommand(
                    sqlReserva,
                    transactionManager.Connection,
                    transactionManager.Transaction))
                {
                    command.Parameters.Add(
                        "@ReservaId",
                        SqlDbType.Int).Value = checkIn.ReservaId;

                    using var reader = command.ExecuteReader();

                    if (!reader.Read())
                    {
                        throw new InvalidOperationException(
                            "La reserva indicada no existe.");
                    }

                    reservaId = reader.GetInt32(
                        reader.GetOrdinal("ReservaId"));

                    clienteId = reader.GetInt32(
                        reader.GetOrdinal("ClienteId"));

                    habitacionId = reader.GetInt32(
                        reader.GetOrdinal("HabitacionId"));

                    string estadoReserva = reader.GetString(
                        reader.GetOrdinal("Estado"));

                    if (estadoReserva == "Cancelada")
                    {
                        throw new InvalidOperationException(
                            "La reserva se encuentra cancelada.");
                    }

                    if (estadoReserva == "Completada")
                    {
                        throw new InvalidOperationException(
                            "La reserva ya fue completada.");
                    }

                    if (clienteId != checkIn.ClienteId)
                    {
                        throw new InvalidOperationException(
                            "El cliente no coincide con la reserva.");
                    }

                    if (habitacionId != checkIn.HabitacionId)
                    {
                        throw new InvalidOperationException(
                            "La habitación no coincide con la reserva.");
                    }
                }

                // =====================================================
                // 2. VERIFICAR QUE NO EXISTA OTRA ESTANCIA ACTIVA
                // =====================================================

                const string sqlEstanciaActiva = """
                    SELECT COUNT(*)
                    FROM Estancia
                    WHERE ReservaId = @ReservaId
                      AND Estado = 'Activa';
                    """;

                using (var command = new SqlCommand(
                    sqlEstanciaActiva,
                    transactionManager.Connection,
                    transactionManager.Transaction))
                {
                    command.Parameters.Add(
                        "@ReservaId",
                        SqlDbType.Int).Value = reservaId;

                    int cantidad = (int)command.ExecuteScalar()!;

                    if (cantidad > 0)
                    {
                        throw new InvalidOperationException(
                            "La reserva ya tiene una estancia activa.");
                    }
                }

                // =====================================================
                // 3. VERIFICAR ESTADO DE LA HABITACIÓN
                // =====================================================

                const string sqlHabitacion = """
                    SELECT Estado
                    FROM Habitacion
                    WHERE HabitacionId = @HabitacionId;
                    """;

                string estadoHabitacion;

                using (var command = new SqlCommand(
                    sqlHabitacion,
                    transactionManager.Connection,
                    transactionManager.Transaction))
                {
                    command.Parameters.Add(
                        "@HabitacionId",
                        SqlDbType.Int).Value = habitacionId;

                    object? resultado = command.ExecuteScalar();

                    if (resultado == null)
                    {
                        throw new InvalidOperationException(
                            "La habitación no existe.");
                    }

                    estadoHabitacion = resultado.ToString()!;
                }

                if (estadoHabitacion != "Reservada")
                {
                    throw new InvalidOperationException(
                        "La habitación no se encuentra en estado Reservada.");
                }
            }
            else
            {
                // =====================================================
                // 4. CHECK-IN SIN RESERVA
                // =====================================================

                if (checkIn.ClienteId <= 0)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un cliente.");
                }

                if (checkIn.HabitacionId <= 0)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar una habitación.");
                }

                // =====================================================
                // 5. VERIFICAR HABITACIÓN DISPONIBLE
                // =====================================================

                const string sqlHabitacion = """
                    SELECT Estado
                    FROM Habitacion
                    WHERE HabitacionId = @HabitacionId;
                    """;

                string estadoHabitacion;

                using (var command = new SqlCommand(
                    sqlHabitacion,
                    transactionManager.Connection,
                    transactionManager.Transaction))
                {
                    command.Parameters.Add(
                        "@HabitacionId",
                        SqlDbType.Int).Value = checkIn.HabitacionId;

                    object? resultado = command.ExecuteScalar();

                    if (resultado == null)
                    {
                        throw new InvalidOperationException(
                            "La habitación no existe.");
                    }

                    estadoHabitacion = resultado.ToString()!;
                }

                if (estadoHabitacion != "Disponible")
                {
                    throw new InvalidOperationException(
                        "La habitación seleccionada no está disponible.");
                }

                clienteId = checkIn.ClienteId;
                habitacionId = checkIn.HabitacionId;

                // =====================================================
                // 6. CREAR RESERVA AUTOMÁTICAMENTE
                // =====================================================

                const string sqlReservaInsert = """
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

                using (var command = new SqlCommand(
                    sqlReservaInsert,
                    transactionManager.Connection,
                    transactionManager.Transaction))
                {
                    command.Parameters.Add(
                        "@ClienteId",
                        SqlDbType.Int).Value = clienteId;

                    command.Parameters.Add(
                        "@HabitacionId",
                        SqlDbType.Int).Value = habitacionId;

                    command.Parameters.Add(
                        "@FechaReserva",
                        SqlDbType.DateTime2).Value =
                        DateTime.Now;

                    command.Parameters.Add(
                        "@FechaEntrada",
                        SqlDbType.DateTime2).Value =
                        checkIn.FechaCheckIn;

                    command.Parameters.Add(
                        "@FechaSalida",
                        SqlDbType.DateTime2).Value =
                        checkIn.FechaSalida;

                    command.Parameters.Add(
                        "@CantidadHuespedes",
                        SqlDbType.Int).Value =
                        checkIn.CantidadHuespedes;

                    command.Parameters.Add(
                        "@Estado",
                        SqlDbType.VarChar,
                        20).Value = "Confirmada";

                    command.Parameters.Add(
                        "@Observaciones",
                        SqlDbType.VarChar,
                        500).Value =
                        (object?)checkIn.Observaciones ?? DBNull.Value;

                    reservaId = (int)command.ExecuteScalar()!;
                }
            }

            // =========================================================
            // 7. CREAR ESTANCIA ACTIVA
            // =========================================================

            const string sqlEstanciaInsert = """
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
                    NULL,
                    'Activa',
                    @Observaciones
                );
                """;

            int estanciaId;

            using (var command = new SqlCommand(
                sqlEstanciaInsert,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@ReservaId",
                    SqlDbType.Int).Value = reservaId;

                command.Parameters.Add(
                    "@FechaCheckIn",
                    SqlDbType.DateTime2).Value =
                    checkIn.FechaCheckIn;

                command.Parameters.Add(
                    "@Observaciones",
                    SqlDbType.VarChar,
                    500).Value =
                    (object?)checkIn.Observaciones ?? DBNull.Value;

                estanciaId = (int)command.ExecuteScalar()!;
            }

            // =========================================================
            // 8. CAMBIAR HABITACIÓN A OCUPADA
            // =========================================================

            const string sqlHabitacionUpdate = """
                UPDATE Habitacion
                SET Estado = 'Ocupada'
                WHERE HabitacionId = @HabitacionId;
                """;

            using (var command = new SqlCommand(
                sqlHabitacionUpdate,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@HabitacionId",
                    SqlDbType.Int).Value = habitacionId;

                command.ExecuteNonQuery();
            }

            // =========================================================
            // 9. CONFIRMAR TODA LA OPERACIÓN
            // =========================================================

            transactionManager.Commit();

            return estanciaId;
        }
        catch
        {
            transactionManager.Rollback();
            throw;
        }
    }
}
