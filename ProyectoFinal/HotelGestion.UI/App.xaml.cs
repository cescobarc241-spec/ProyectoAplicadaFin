using System.Windows;
using HotelGestion.Application.Interfaces;
using HotelGestion.Application.UseCases;
using HotelGestion.Infrastructure.Data;
using HotelGestion.Infrastructure.Repositories;
namespace HotelGestion.UI;

public partial class App : System.Windows.Application
{
    public DbConnectionFactory ConnectionFactory { get; private set; } = null!;

    public IHabitacionService HabitacionService { get; private set; } = null!;

    public ITipoHabitacionService TipoHabitacionService { get; private set; } = null!;

    public IClienteService ClienteService { get; private set; } = null!;

    public IReservaService ReservaService { get; private set; } = null!;

    public IProductoMinibarService ProductoMinibarService { get; private set; } = null!;

    public IConsumoMinibarService ConsumoMinibarService { get; private set; } = null!;

    public IEstanciaService EstanciaService { get; private set; } = null!;
    public IFacturaService FacturaService { get; private set; } = null!;

    public IDetalleFacturaService DetalleFacturaService { get; private set; } = null!;
    public ICheckInService CheckInService { get; private set; } = null!;
    public ICheckOutService CheckOutService { get; private set; } = null!;
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        string server = Environment.MachineName;

        string connectionString =
            $"Data Source={server};Initial Catalog=HotelDB;Integrated Security=True;TrustServerCertificate=True";

        ConnectionFactory =
            new DbConnectionFactory(connectionString);

        // Repository de habitaciones
        var habitacionRepository =
            new HabitacionRepository(ConnectionFactory);

        // Service de habitaciones
        HabitacionService =
            new HabitacionService(habitacionRepository);

        // Repository de tipos de habitación
        var tipoHabitacionRepository =
            new TipoHabitacionRepository(ConnectionFactory);

        // Service de tipos de habitación
        TipoHabitacionService =
            new TipoHabitacionService(tipoHabitacionRepository);

        // Repository de clientes
        var clienteRepository =
            new ClienteRepository(ConnectionFactory);

        // Service de clientes
        ClienteService =
            new ClienteService(clienteRepository);

        // Repository de reservas
        var reservaRepository =
            new ReservaRepository(ConnectionFactory);

        // Service de reservas
        ReservaService =
            new ReservaService(reservaRepository);

        // Repository de estancias
        var estanciaRepository =
            new EstanciaRepository(ConnectionFactory);

        // Service de estancias
        EstanciaService =
            new EstanciaService(estanciaRepository);

        // Repository de productos de minibar
        var productoMinibarRepository =
            new ProductoMinibarRepository(ConnectionFactory);

        // Service de productos de minibar
        ProductoMinibarService =
            new ProductoMinibarService(productoMinibarRepository);

        // Repository de consumos de minibar
        var consumoMinibarRepository =
            new ConsumoMinibarRepository(ConnectionFactory);

        // Service de consumos de minibar
        ConsumoMinibarService =
            new ConsumoMinibarService(consumoMinibarRepository);

        // Repository de facturas
        var facturaRepository =
            new FacturaRepository(ConnectionFactory);

        // Service de facturas
        FacturaService =
            new FacturaService(facturaRepository);

        // Repository de detalles de factura
        var detalleFacturaRepository =
            new DetalleFacturaRepository(ConnectionFactory);

        // Service de detalles de factura
        DetalleFacturaService =
            new DetalleFacturaService(detalleFacturaRepository);

        // Repository de checkouts
        var checkOutRepository =
             new CheckOutRepository(ConnectionFactory);

        // Repository de checkins
        var checkInRepository =
         new CheckInRepository(ConnectionFactory);

        // Service de checkins
        CheckInService =
            new CheckInService(checkInRepository);

        // Service de checkouts
        CheckOutService =
            new CheckOutService(checkOutRepository);
    }
}