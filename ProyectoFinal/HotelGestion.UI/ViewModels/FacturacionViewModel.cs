using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Application.UseCases;
using System;
using System.Collections.ObjectModel;

namespace HotelGestion.UI.ViewModels;

public class FacturacionViewModel : BaseViewModel
{
    

    // =========================================================
    // SERVICIOS
    // =========================================================

    private readonly IFacturaService _facturaService;
    private readonly IDetalleFacturaService _detalleFacturaService;
    private readonly IEstanciaService _estanciaService;
    private readonly ICheckOutService _checkOutService;
    private readonly IReservaService _reservaService;
    private readonly IHabitacionService _habitacionService;

    private readonly IClienteService _clienteService;

    // =========================================================
    // COLECCIONES
    // =========================================================

    public ObservableCollection<FacturaDto> Facturas { get; } = new();

    public ObservableCollection<DetalleFacturaDto> Detalles { get; } = new();

    public ObservableCollection<EstanciaDto> Estancias { get; } = new();

    // =========================================================
    // FACTURA SELECCIONADA
    // =========================================================

    private FacturaDto? _facturaSeleccionada;

    public FacturaDto? FacturaSeleccionada
    {
        get => _facturaSeleccionada;

        set
        {
            if (!SetProperty(ref _facturaSeleccionada, value))
                return;

            if (_facturaSeleccionada != null)
            {
                EstanciaId = _facturaSeleccionada.EstanciaId;
                NumeroFactura = _facturaSeleccionada.NumeroFactura;

                var estancia = _estanciaService
                    .ObtenerPorId(_facturaSeleccionada.EstanciaId);

                if (estancia != null)
                {
                    var reserva = _reservaService
                        .ObtenerPorId(estancia.ReservaId);

                    if (reserva != null)
                    {
                        var cliente = _clienteService
                            .ObtenerPorId(reserva.ClienteId);

                        NombreCliente = cliente != null
                            ? $"{cliente.Nombres} {cliente.Apellidos}"
                            : string.Empty;
                    }
                }

                FechaEmision = _facturaSeleccionada.FechaEmision;
                Subtotal = _facturaSeleccionada.Subtotal;
                Impuesto = _facturaSeleccionada.Impuesto;
                Total = _facturaSeleccionada.Total;
                MetodoPago = _facturaSeleccionada.MetodoPago;
                Estado = _facturaSeleccionada.Estado;
            }
            else
            {
                NombreCliente = string.Empty;
                EstanciaId = 0;
                NumeroFactura = string.Empty;
                FechaEmision = DateTime.Now;
                Subtotal = 0;
                Impuesto = 0;
                Total = 0;
                MetodoPago = "Efectivo";
                Estado = "Emitida";
            }

            CargarDetalles();
        }
    }

    // =========================================================
    // CAMPOS DE FACTURA
    // =========================================================

    private string _nombreCliente = string.Empty;

    public string NombreCliente
    {
        get => _nombreCliente;
        set => SetProperty(ref _nombreCliente, value);
    }
    private int _estanciaId;


    public int EstanciaId
    {
        get => _estanciaId;
        set => SetProperty(ref _estanciaId, value);
    }

    private string _numeroFactura = string.Empty;

    public string NumeroFactura
    {
        get => _numeroFactura;
        set => SetProperty(ref _numeroFactura, value);
    }

    private DateTime _fechaEmision = DateTime.Now;

    public DateTime FechaEmision
    {
        get => _fechaEmision;
        set => SetProperty(ref _fechaEmision, value);
    }

    private decimal _subtotal;

    public decimal Subtotal
    {
        get => _subtotal;
        set => SetProperty(ref _subtotal, value);
    }

    private decimal _impuesto;

    public decimal Impuesto
    {
        get => _impuesto;
        set => SetProperty(ref _impuesto, value);
    }

    private decimal _total;

    public decimal Total
    {
        get => _total;
        set => SetProperty(ref _total, value);
    }

    private string _metodoPago = "Efectivo";

    public string MetodoPago
    {
        get => _metodoPago;
        set => SetProperty(ref _metodoPago, value);
    }

    private string _estado = "Emitida";

    public string Estado
    {
        get => _estado;
        set => SetProperty(ref _estado, value);
    }

    // =========================================================
    // DETALLE SELECCIONADO
    // =========================================================

    private DetalleFacturaDto? _detalleSeleccionado;

    public DetalleFacturaDto? DetalleSeleccionado
    {
        get => _detalleSeleccionado;

        set
        {
            if (!SetProperty(ref _detalleSeleccionado, value))
                return;

            if (_detalleSeleccionado != null)
            {
                DetalleFacturaId = _detalleSeleccionado.DetalleFacturaId;
                FacturaId = _detalleSeleccionado.FacturaId;
                Descripcion = _detalleSeleccionado.Descripcion;
                Cantidad = _detalleSeleccionado.Cantidad;
                PrecioUnitario = _detalleSeleccionado.PrecioUnitario;
                SubtotalDetalle = _detalleSeleccionado.Subtotal;
            }
            else
            {
                DetalleFacturaId = 0;
                FacturaId = 0;
                Descripcion = string.Empty;
                Cantidad = 1;
                PrecioUnitario = 0;
                SubtotalDetalle = 0;
            }
        }
    }

    // =========================================================
    // CAMPOS DE DETALLE
    // =========================================================

    private int _detalleFacturaId;

    public int DetalleFacturaId
    {
        get => _detalleFacturaId;
        set => SetProperty(ref _detalleFacturaId, value);
    }

    private int _facturaId;

    public int FacturaId
    {
        get => _facturaId;
        set => SetProperty(ref _facturaId, value);
    }

    private string _descripcion = string.Empty;

    public string Descripcion
    {
        get => _descripcion;
        set => SetProperty(ref _descripcion, value);
    }

    private int _cantidad = 1;

    public int Cantidad
    {
        get => _cantidad;
        set => SetProperty(ref _cantidad, value);
    }

    private decimal _precioUnitario;

    public decimal PrecioUnitario
    {
        get => _precioUnitario;
        set => SetProperty(ref _precioUnitario, value);
    }

    private decimal _subtotalDetalle;

    public decimal SubtotalDetalle
    {
        get => _subtotalDetalle;
        set => SetProperty(ref _subtotalDetalle, value);
    }

    // =========================================================
    // DATOS PARA CHECK-OUT
    // =========================================================

    private int _estanciaIdCheckOut;

    public int EstanciaIdCheckOut
    {
        get => _estanciaIdCheckOut;
        set => SetProperty(ref _estanciaIdCheckOut, value);
    }

    private string _metodoPagoCheckOut = "Efectivo";

    public string MetodoPagoCheckOut
    {
        get => _metodoPagoCheckOut;
        set => SetProperty(ref _metodoPagoCheckOut, value);
    }

    // =========================================================
    // COMANDOS
    // =========================================================

    public RelayCommand CargarCommand { get; }

    public RelayCommand VerDetalleCommand { get; }

    public RelayCommand EjecutarCheckOutCommand { get; }

    // =========================================================
    // CONSTRUCTOR
    // =========================================================

    public FacturacionViewModel(
        IFacturaService facturaService,
        IDetalleFacturaService detalleFacturaService,
        IEstanciaService estanciaService,
        ICheckOutService checkOutService,
        IReservaService reservaService,
        IHabitacionService habitacionService,
        IClienteService clienteService)
    {
        _facturaService = facturaService;
        _detalleFacturaService = detalleFacturaService;
        _estanciaService = estanciaService;
        _checkOutService = checkOutService;
        _reservaService = reservaService;
        _habitacionService = habitacionService;
        _clienteService = clienteService;

        // -----------------------------------------------------
        // COMANDOS DE FACTURA
        // -----------------------------------------------------

        CargarCommand =
            new RelayCommand(CargarDatos);

        
        VerDetalleCommand =
            new RelayCommand(VerDetalleFactura);

        // -----------------------------------------------------
        // CHECK-OUT
        // -----------------------------------------------------

        EjecutarCheckOutCommand =
            new RelayCommand(EjecutarCheckOut);

        // -----------------------------------------------------
        // CARGA INICIAL
        // -----------------------------------------------------

        CargarDatos();
    }

    // =========================================================
    // CARGAR DATOS
    // =========================================================

    private void CargarDatos()
    {
        CargarFacturas();
        CargarEstancias();
    }

    // =========================================================
    // VER DETALLE DE FACTURA
    // =========================================================

    private void VerDetalleFactura()
    {
        if (FacturaSeleccionada == null)
        {
            System.Windows.MessageBox.Show(
                "Seleccione una factura para ver su detalle.",
                "Facturación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);

            return;
        }

        try
        {
            var estancia = _estanciaService
                .ObtenerPorId(FacturaSeleccionada.EstanciaId);

            if (estancia == null)
            {
                System.Windows.MessageBox.Show(
                    "No se encontró la estancia asociada a la factura.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            var reserva = _reservaService
                .ObtenerPorId(estancia.ReservaId);

            if (reserva == null)
            {
                System.Windows.MessageBox.Show(
                    "No se encontró la reserva asociada a la estancia.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            var habitacion = _habitacionService
                .ObtenerPorId(reserva.HabitacionId);

            if (habitacion == null)
            {
                System.Windows.MessageBox.Show(
                    "No se encontró la habitación asociada.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            // Cargar detalles de la factura seleccionada
            var detalles = _detalleFacturaService
                .ObtenerTodos()
                .Where(d =>
                    d.FacturaId == FacturaSeleccionada.FacturaId)
                .ToList();

            string mensaje =
                $"FACTURA: {FacturaSeleccionada.NumeroFactura}\n" +
                $"CLIENTE: {NombreCliente}\n" +
                $"ESTANCIA: {FacturaSeleccionada.EstanciaId}\n" +
                $"HABITACIÓN: {habitacion.Numero}\n" +
                $"FECHA: {FacturaSeleccionada.FechaEmision:dd/MM/yyyy HH:mm}\n" +
                $"MÉTODO DE PAGO: {FacturaSeleccionada.MetodoPago}\n\n" +

                "DETALLE\n" +
                "----------------------------------------\n";

            foreach (var detalle in detalles)
            {
                mensaje +=
                    $"{detalle.Descripcion}\n" +
                    $"Cantidad: {detalle.Cantidad}    " +
                    $"Precio: S/ {detalle.PrecioUnitario:N2}\n" +
                    $"Subtotal: S/ {detalle.Subtotal:N2}\n\n";
            }

            mensaje +=
                "----------------------------------------\n" +
                $"SUBTOTAL: S/ {FacturaSeleccionada.Subtotal:N2}\n" +
                $"IGV: S/ {FacturaSeleccionada.Impuesto:N2}\n" +
                $"TOTAL: S/ {FacturaSeleccionada.Total:N2}";

            System.Windows.MessageBox.Show(
                mensaje,
                "Detalle de factura",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo consultar el detalle de la factura.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }
    private void CargarFacturas()
    {
        Facturas.Clear();

        var lista = _facturaService.ObtenerTodos();

        foreach (var factura in lista)
        {
            Facturas.Add(factura);
        }
    }

    private void CargarEstancias()
    {
        Estancias.Clear();

        var lista = _estanciaService.ObtenerTodos();

        foreach (var estancia in lista)
        {
            Estancias.Add(estancia);
        }
    }

    private void CargarDetalles()
    {
        Detalles.Clear();

        if (FacturaSeleccionada == null)
            return;

        var lista =
            _detalleFacturaService.ObtenerTodos();

        foreach (var detalle in lista)
        {
            if (detalle.FacturaId ==
                FacturaSeleccionada.FacturaId)
            {
                Detalles.Add(detalle);
            }
        }
    }



    // =========================================================
    // CHECK-OUT
    // =========================================================

    private void EjecutarCheckOut()
    {

        try
        {
            if (EstanciaIdCheckOut <= 0)
            {
                System.Windows.MessageBox.Show(
                    "Debe seleccionar una estancia válida.",
                    "Check-Out",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(
                MetodoPagoCheckOut))
            {
                System.Windows.MessageBox.Show(
                    "Debe seleccionar un método de pago.",
                    "Check-Out",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            var checkOut = new CheckOutDto
            {
                EstanciaId = EstanciaIdCheckOut,
                MetodoPago = MetodoPagoCheckOut,
                // SOLO PARA LA PRUEBA DE ROLLBACK
                //ForzarError = true
            };

            _checkOutService.Ejecutar(checkOut);

            // Actualizamos la información de pantalla
            CargarFacturas();
            CargarEstancias();

            System.Windows.MessageBox.Show(
                "Check-Out realizado correctamente.\n\n" +
                "Se generó la factura,\n" +
                "se liquidó el minibar,\n" +
                "y la habitación pasó a estado Limpieza.",
                "Check-Out",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo realizar el Check-Out.\n\n" +
                ex.Message,
                "Error en Check-Out",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }
}