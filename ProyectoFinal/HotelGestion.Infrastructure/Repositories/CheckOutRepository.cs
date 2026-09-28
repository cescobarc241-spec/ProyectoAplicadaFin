using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Infrastructure.Data;
using HotelGestion.Infrastructure.Transactions;

namespace HotelGestion.Infrastructure.Repositories;

public class CheckOutRepository : ICheckOutRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public CheckOutRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Ejecutar(CheckOutDto checkOut)
    {
        using var transactionManager =
            new TransactionManager(_connectionFactory);

        try
        {
            // =========================================================
            // 1. OBTENER LA ESTANCIA Y LOS DATOS DE LA HABITACIÓN
            // =========================================================

            int reservaId;
            int habitacionId;
            string estadoEstancia;
            DateTime fechaCheckIn;
            decimal precioPorNoche;

            const string sqlEstancia = """
                SELECT
                    e.ReservaId,
                    e.FechaCheckIn,
                    e.Estado,
                    r.HabitacionId,
                    th.PrecioPorNoche
                FROM Estancia e
                INNER JOIN Reserva r
                    ON e.ReservaId = r.ReservaId
                INNER JOIN Habitacion h
                    ON r.HabitacionId = h.HabitacionId
                INNER JOIN TipoHabitacion th
                    ON h.TipoHabitacionId = th.TipoHabitacionId
                WHERE e.EstanciaId = @EstanciaId;
                """;

            using (var command = new SqlCommand(
                sqlEstancia,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@EstanciaId",
                    System.Data.SqlDbType.Int).Value = checkOut.EstanciaId;

                using var reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    throw new InvalidOperationException(
                        "La estancia indicada no existe.");
                }

                reservaId = reader.GetInt32(
                    reader.GetOrdinal("ReservaId"));

                habitacionId = reader.GetInt32(
                    reader.GetOrdinal("HabitacionId"));

                fechaCheckIn = reader.GetDateTime(
                    reader.GetOrdinal("FechaCheckIn"));

                estadoEstancia = reader.GetString(
                    reader.GetOrdinal("Estado"));

                precioPorNoche = reader.GetDecimal(
                    reader.GetOrdinal("PrecioPorNoche"));
            }

            if (estadoEstancia != "Activa")
            {
                throw new InvalidOperationException(
                    "La estancia no se encuentra activa.");
            }

            // =========================================================
            // 2. CALCULAR LOS DÍAS / NOCHES DE ESTANCIA
            // =========================================================

            DateTime fechaCheckOut = DateTime.Now;

            int cantidadNoches =
                (fechaCheckOut.Date - fechaCheckIn.Date).Days;

            if (cantidadNoches <= 0)
            {
                cantidadNoches = 1;
            }

            decimal subtotalEstadia =
                cantidadNoches * precioPorNoche;

            // =========================================================
            // 3. OBTENER Y LIQUIDAR LOS CONSUMOS DE MINIBAR
            // =========================================================

            decimal subtotalMinibar = 0m;

            var consumos = new List<ConsumoTemporal>();

            const string sqlConsumos = """
                SELECT
                    c.ConsumoMinibarId,
                    c.Cantidad,
                    c.PrecioUnitario,
                    p.Nombre
                FROM ConsumoMinibar c
                INNER JOIN ProductoMinibar p
                    ON c.ProductoMinibarId = p.ProductoMinibarId
                WHERE c.EstanciaId = @EstanciaId
                ORDER BY c.ConsumoMinibarId;
                """;

            using (var command = new SqlCommand(
                sqlConsumos,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@EstanciaId",
                    System.Data.SqlDbType.Int).Value = checkOut.EstanciaId;

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int cantidad = reader.GetInt32(
                        reader.GetOrdinal("Cantidad"));

                    decimal precioUnitario = reader.GetDecimal(
                        reader.GetOrdinal("PrecioUnitario"));

                    string nombre = reader.GetString(
                        reader.GetOrdinal("Nombre"));

                    decimal subtotalConsumo =
                        cantidad * precioUnitario;

                    subtotalMinibar += subtotalConsumo;

                    consumos.Add(new ConsumoTemporal
                    {
                        Nombre = nombre,
                        Cantidad = cantidad,
                        PrecioUnitario = precioUnitario,
                        Subtotal = subtotalConsumo
                    });
                }
            }

            // =========================================================
            // 4. CALCULAR TOTALES
            // =========================================================

            decimal subtotal =
                subtotalEstadia + subtotalMinibar;

            decimal impuesto =
                Math.Round(subtotal * 0.18m, 2);

            decimal total =
                subtotal + impuesto;

            // =========================================================
            // 5. GENERAR NÚMERO DE FACTURA
            // =========================================================

            string numeroFactura =
                $"F-{DateTime.Now:yyyyMMddHHmmssfff}";

            // =========================================================
            // 6. INSERTAR FACTURA
            // =========================================================

            int facturaId;

            const string sqlFactura = """
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

            using (var command = new SqlCommand(
                sqlFactura,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@EstanciaId",
                    System.Data.SqlDbType.Int).Value = checkOut.EstanciaId;

                command.Parameters.Add(
                    "@NumeroFactura",
                    System.Data.SqlDbType.VarChar,
                    30).Value = numeroFactura;

                command.Parameters.Add(
                    "@FechaEmision",
                    System.Data.SqlDbType.DateTime2).Value =
                    fechaCheckOut;

                var subtotalParameter =
                    command.Parameters.Add(
                        "@Subtotal",
                        System.Data.SqlDbType.Decimal);

                subtotalParameter.Precision = 12;
                subtotalParameter.Scale = 2;
                subtotalParameter.Value = subtotal;

                var impuestoParameter =
                    command.Parameters.Add(
                        "@Impuesto",
                        System.Data.SqlDbType.Decimal);

                impuestoParameter.Precision = 12;
                impuestoParameter.Scale = 2;
                impuestoParameter.Value = impuesto;

                var totalParameter =
                    command.Parameters.Add(
                        "@Total",
                        System.Data.SqlDbType.Decimal);

                totalParameter.Precision = 12;
                totalParameter.Scale = 2;
                totalParameter.Value = total;

                command.Parameters.Add(
                    "@MetodoPago",
                    System.Data.SqlDbType.VarChar,
                    20).Value = checkOut.MetodoPago;

                command.Parameters.Add(
                    "@Estado",
                    System.Data.SqlDbType.VarChar,
                    20).Value = "Emitida";

                facturaId =
                    (int)command.ExecuteScalar()!;
            }

            // =========================================================
            // 7. INSERTAR DETALLE DE ESTADÍA
            // =========================================================

            const string sqlDetalle = """
                INSERT INTO DetalleFactura
                (
                    FacturaId,
                    Descripcion,
                    Cantidad,
                    PrecioUnitario,
                    Subtotal
                )
                VALUES
                (
                    @FacturaId,
                    @Descripcion,
                    @Cantidad,
                    @PrecioUnitario,
                    @Subtotal
                );
                """;

            using (var command = new SqlCommand(
                sqlDetalle,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@FacturaId",
                    System.Data.SqlDbType.Int).Value = facturaId;

                command.Parameters.Add(
                    "@Descripcion",
                    System.Data.SqlDbType.VarChar,
                    250).Value =
                    "Hospedaje";

                command.Parameters.Add(
                    "@Cantidad",
                    System.Data.SqlDbType.Int).Value =
                    cantidadNoches;

                var precioParameter =
                    command.Parameters.Add(
                        "@PrecioUnitario",
                        System.Data.SqlDbType.Decimal);

                precioParameter.Precision = 12;
                precioParameter.Scale = 2;
                precioParameter.Value = precioPorNoche;

                var subtotalParameter =
                    command.Parameters.Add(
                        "@Subtotal",
                        System.Data.SqlDbType.Decimal);

                subtotalParameter.Precision = 12;
                subtotalParameter.Scale = 2;
                subtotalParameter.Value = subtotalEstadia;

                command.ExecuteNonQuery();
            }

            // =========================================================
            // 8. INSERTAR DETALLES DEL MINIBAR
            // =========================================================

            foreach (var consumo in consumos)
            {
                using var command = new SqlCommand(
                    sqlDetalle,
                    transactionManager.Connection,
                    transactionManager.Transaction);

                command.Parameters.Add(
                    "@FacturaId",
                    System.Data.SqlDbType.Int).Value = facturaId;

                command.Parameters.Add(
                    "@Descripcion",
                    System.Data.SqlDbType.VarChar,
                    250).Value =
                    $"Minibar - {consumo.Nombre}";

                command.Parameters.Add(
                    "@Cantidad",
                    System.Data.SqlDbType.Int).Value =
                    consumo.Cantidad;

                var precioParameter =
                    command.Parameters.Add(
                        "@PrecioUnitario",
                        System.Data.SqlDbType.Decimal);

                precioParameter.Precision = 12;
                precioParameter.Scale = 2;
                precioParameter.Value =
                    consumo.PrecioUnitario;

                var subtotalParameter =
                    command.Parameters.Add(
                        "@Subtotal",
                        System.Data.SqlDbType.Decimal);

                subtotalParameter.Precision = 12;
                subtotalParameter.Scale = 2;
                subtotalParameter.Value =
                    consumo.Subtotal;

                command.ExecuteNonQuery();
            }

            // =========================================================
            // 9. FINALIZAR ESTANCIA
            // =========================================================

            const string sqlEstanciaUpdate = """
                UPDATE Estancia
                SET
                    FechaCheckOut = @FechaCheckOut,
                    Estado = 'Finalizada'
                WHERE EstanciaId = @EstanciaId;
                """;

            using (var command = new SqlCommand(
                sqlEstanciaUpdate,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@FechaCheckOut",
                    System.Data.SqlDbType.DateTime2).Value =
                    fechaCheckOut;

                command.Parameters.Add(
                    "@EstanciaId",
                    System.Data.SqlDbType.Int).Value =
                    checkOut.EstanciaId;

                command.ExecuteNonQuery();
            }

            // =========================================================
            // 10. COMPLETAR LA RESERVA
            // =========================================================

            const string sqlReservaUpdate = """
                UPDATE Reserva
                SET
                    Estado = 'Completada'
                WHERE ReservaId = @ReservaId;
                """;

            using (var command = new SqlCommand(
                sqlReservaUpdate,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@ReservaId",
                    System.Data.SqlDbType.Int).Value =
                    reservaId;

                command.ExecuteNonQuery();
            }

            // =========================================================
            // 11. CAMBIAR HABITACIÓN A LIMPIEZA
            // =========================================================

            const string sqlHabitacionUpdate = """
                UPDATE Habitacion
                SET
                    Estado = 'Limpieza'
                WHERE HabitacionId = @HabitacionId;
                """;

            using (var command = new SqlCommand(
                sqlHabitacionUpdate,
                transactionManager.Connection,
                transactionManager.Transaction))
            {
                command.Parameters.Add(
                    "@HabitacionId",
                    System.Data.SqlDbType.Int).Value =
                    habitacionId;

                command.ExecuteNonQuery();
            }


            // =========================================================
            // 11.1. PRUEBA DE ROLLBACK
            // =========================================================
            //SOLO PARA LA PRUEBA DE ROLLBACK
            if (checkOut.ForzarError)
            {
                throw new InvalidOperationException(
                   "Fallo controlado para demostrar Rollback.");
            }


            // =========================================================
            // 12. CONFIRMAR TODA LA OPERACIÓN
            // =========================================================

            transactionManager.Commit();
        }
        catch
        {
            transactionManager.Rollback();
            throw;
        }
    }

    private class ConsumoTemporal
    {
        public string Nombre { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}