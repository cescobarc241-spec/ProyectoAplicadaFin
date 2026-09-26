using HotelGestion.Domain.Entities;
using HotelGestion.Infrastructure.Data;
using HotelGestion.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HotelGestion.UI
{
    public class DetalleFacturaRepositoryTest
    {
        public static void Probar(DbConnectionFactory connectionFactory)
        {
            var facturaRepository = new FacturaRepository(connectionFactory);
            var detalleRepository = new DetalleFacturaRepository(connectionFactory);

            Debug.WriteLine("=== PRUEBA DETALLE FACTURA ===");

            // 1. Obtener todos los detalles
            var detalles = detalleRepository.ObtenerTodos();

            Debug.WriteLine($"Detalles encontrados: {detalles.Count}");

            // 2. Crear factura temporal
            var facturaTemporal = new Factura
            {
                EstanciaId = 1,
                NumeroFactura = "TEST-DETALLE-001",
                FechaEmision = DateTime.Now,
                Subtotal = 50m,
                Impuesto = 9m,
                Total = 59m,
                MetodoPago = "Efectivo",
                Estado = "Emitida"
            };

            int facturaId = facturaRepository.Insertar(facturaTemporal);

            Debug.WriteLine(
                $"Factura temporal creada con ID: {facturaId}");

            // 3. Insertar detalle
            var detalleTemporal = new DetalleFactura
            {
                FacturaId = facturaId,
                Descripcion = "Consumo de minibar - Agua mineral",
                Cantidad = 2,
                PrecioUnitario = 5.00m,
                Subtotal = 10.00m
            };

            int detalleId = detalleRepository.Insertar(detalleTemporal);

            Debug.WriteLine(
                $"Detalle insertado con ID: {detalleId}");

            // 4. Obtener detalle por ID
            var detalleObtenido =
                detalleRepository.ObtenerPorId(detalleId);

            Debug.WriteLine(
                $"Detalle obtenido: " +
                $"{detalleObtenido?.Descripcion} | " +
                $"Cantidad: {detalleObtenido?.Cantidad} | " +
                $"Subtotal: {detalleObtenido?.Subtotal}");

            // 5. Actualizar detalle
            detalleObtenido!.Descripcion =
                "Consumo de minibar - Agua mineral y gaseosa";

            detalleObtenido.Cantidad = 3;
            detalleObtenido.PrecioUnitario = 6.50m;
            detalleObtenido.Subtotal = 19.50m;

            detalleRepository.Actualizar(detalleObtenido);

            var detalleActualizado =
                detalleRepository.ObtenerPorId(detalleId);

            Debug.WriteLine(
                $"Detalle actualizado: " +
                $"{detalleActualizado?.Descripcion} | " +
                $"Cantidad: {detalleActualizado?.Cantidad} | " +
                $"Subtotal: {detalleActualizado?.Subtotal}");

            // 6. Eliminar detalle
            detalleRepository.Eliminar(detalleId);

            var detalleEliminado =
                detalleRepository.ObtenerPorId(detalleId);

            Debug.WriteLine(
                $"Detalle después de eliminar: " +
                $"{(detalleEliminado == null ? "NULL (correcto)" : "ERROR")}");

            // 7. Eliminar factura temporal
            facturaRepository.Eliminar(facturaId);

            var facturaEliminada =
                facturaRepository.ObtenerPorId(facturaId);

            Debug.WriteLine(
                $"Factura temporal después de eliminar: " +
                $"{(facturaEliminada == null ? "NULL (correcto)" : "ERROR")}");

            Debug.WriteLine("=== FIN PRUEBA DETALLE FACTURA ===");
        }
    }
}
