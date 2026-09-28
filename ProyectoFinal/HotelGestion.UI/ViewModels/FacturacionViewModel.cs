using System;
using System.Collections.ObjectModel;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

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
                FechaEmision = _facturaSeleccionada.FechaEmision;
                Subtotal = _facturaSeleccionada.Subtotal;
                Impuesto = _facturaSeleccionada.Impuesto;
                Total = _facturaSeleccionada.Total;
                MetodoPago = _facturaSeleccionada.MetodoPago;
                Estado = _facturaSeleccionada.Estado;
            }
            else
            {
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

    public RelayCommand NuevoCommand { get; }

    public RelayCommand GuardarCommand { get; }

    public RelayCommand ActualizarCommand { get; }

    public RelayCommand EliminarCommand { get; }

    public RelayCommand NuevoDetalleCommand { get; }

    public RelayCommand GuardarDetalleCommand { get; }

    public RelayCommand ActualizarDetalleCommand { get; }

    public RelayCommand EliminarDetalleCommand { get; }

    public RelayCommand EjecutarCheckOutCommand { get; }

    // =========================================================
    // CONSTRUCTOR
    // =========================================================

    public FacturacionViewModel(
        IFacturaService facturaService,
        IDetalleFacturaService detalleFacturaService,
        IEstanciaService estanciaService,
        ICheckOutService checkOutService)
    {
        _facturaService = facturaService;
        _detalleFacturaService = detalleFacturaService;
        _estanciaService = estanciaService;
        _checkOutService = checkOutService;

        // -----------------------------------------------------
        // COMANDOS DE FACTURA
        // -----------------------------------------------------

        CargarCommand =
            new RelayCommand(CargarDatos);

        NuevoCommand =
            new RelayCommand(NuevoFactura);

        GuardarCommand =
            new RelayCommand(GuardarFactura);

        ActualizarCommand =
            new RelayCommand(ActualizarFactura);

        EliminarCommand =
            new RelayCommand(EliminarFactura);

        // -----------------------------------------------------
        // COMANDOS DE DETALLE
        // -----------------------------------------------------

        NuevoDetalleCommand =
            new RelayCommand(NuevoDetalle);

        GuardarDetalleCommand =
            new RelayCommand(GuardarDetalle);

        ActualizarDetalleCommand =
            new RelayCommand(ActualizarDetalle);

        EliminarDetalleCommand =
            new RelayCommand(EliminarDetalle);

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
    // NUEVA FACTURA
    // =========================================================

    private void NuevoFactura()
    {
        FacturaSeleccionada = null;

        EstanciaId = 0;
        NumeroFactura = string.Empty;
        FechaEmision = DateTime.Now;
        Subtotal = 0;
        Impuesto = 0;
        Total = 0;
        MetodoPago = "Efectivo";
        Estado = "Emitida";

        Detalles.Clear();
    }

    // =========================================================
    // GUARDAR FACTURA
    // =========================================================

    private void GuardarFactura()
    {
        try
        {
            var factura = new FacturaDto
            {
                EstanciaId = EstanciaId,
                NumeroFactura = NumeroFactura,
                FechaEmision = FechaEmision,
                Subtotal = Subtotal,
                Impuesto = Impuesto,
                Total = Total,
                MetodoPago = MetodoPago,
                Estado = Estado
            };

            int id =
                _facturaService.Registrar(factura);

            factura.FacturaId = id;

            Facturas.Add(factura);

            System.Windows.MessageBox.Show(
                "Factura guardada correctamente.",
                "Facturación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo guardar la factura.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    // =========================================================
    // ACTUALIZAR FACTURA
    // =========================================================

    private void ActualizarFactura()
    {
        try
        {
            if (FacturaSeleccionada == null)
            {
                System.Windows.MessageBox.Show(
                    "Seleccione una factura.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            FacturaSeleccionada.EstanciaId = EstanciaId;
            FacturaSeleccionada.NumeroFactura = NumeroFactura;
            FacturaSeleccionada.FechaEmision = FechaEmision;
            FacturaSeleccionada.Subtotal = Subtotal;
            FacturaSeleccionada.Impuesto = Impuesto;
            FacturaSeleccionada.Total = Total;
            FacturaSeleccionada.MetodoPago = MetodoPago;
            FacturaSeleccionada.Estado = Estado;

            _facturaService.Actualizar(
                FacturaSeleccionada);

            CargarFacturas();

            System.Windows.MessageBox.Show(
                "Factura actualizada correctamente.",
                "Facturación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo actualizar la factura.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    // =========================================================
    // ELIMINAR FACTURA
    // =========================================================

    private void EliminarFactura()
    {
        try
        {
            if (FacturaSeleccionada == null)
            {
                System.Windows.MessageBox.Show(
                    "Seleccione una factura.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            _facturaService.Eliminar(
                FacturaSeleccionada.FacturaId);

            Facturas.Remove(FacturaSeleccionada);

            Detalles.Clear();

            System.Windows.MessageBox.Show(
                "Factura eliminada correctamente.",
                "Facturación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo eliminar la factura.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    // =========================================================
    // NUEVO DETALLE
    // =========================================================

    private void NuevoDetalle()
    {
        DetalleSeleccionado = null;

        DetalleFacturaId = 0;

        if (FacturaSeleccionada != null)
            FacturaId = FacturaSeleccionada.FacturaId;
        else
            FacturaId = 0;

        Descripcion = string.Empty;
        Cantidad = 1;
        PrecioUnitario = 0;
        SubtotalDetalle = 0;
    }

    // =========================================================
    // GUARDAR DETALLE
    // =========================================================

    private void GuardarDetalle()
    {
        try
        {
            if (FacturaSeleccionada == null)
            {
                System.Windows.MessageBox.Show(
                    "Seleccione una factura.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            var detalle = new DetalleFacturaDto
            {
                FacturaId =
                    FacturaSeleccionada.FacturaId,

                Descripcion = Descripcion,

                Cantidad = Cantidad,

                PrecioUnitario = PrecioUnitario,

                Subtotal = SubtotalDetalle
            };

            int id =
                _detalleFacturaService.Registrar(detalle);

            detalle.DetalleFacturaId = id;

            Detalles.Add(detalle);

            System.Windows.MessageBox.Show(
                "Detalle guardado correctamente.",
                "Facturación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo guardar el detalle.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    // =========================================================
    // ACTUALIZAR DETALLE
    // =========================================================

    private void ActualizarDetalle()
    {
        try
        {
            if (DetalleSeleccionado == null)
            {
                System.Windows.MessageBox.Show(
                    "Seleccione un detalle.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            DetalleSeleccionado.FacturaId = FacturaId;
            DetalleSeleccionado.Descripcion = Descripcion;
            DetalleSeleccionado.Cantidad = Cantidad;
            DetalleSeleccionado.PrecioUnitario = PrecioUnitario;
            DetalleSeleccionado.Subtotal = SubtotalDetalle;

            _detalleFacturaService.Actualizar(
                DetalleSeleccionado);

            CargarDetalles();

            System.Windows.MessageBox.Show(
                "Detalle actualizado correctamente.",
                "Facturación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo actualizar el detalle.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    // =========================================================
    // ELIMINAR DETALLE
    // =========================================================

    private void EliminarDetalle()
    {
        try
        {
            if (DetalleSeleccionado == null)
            {
                System.Windows.MessageBox.Show(
                    "Seleccione un detalle.",
                    "Facturación",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            _detalleFacturaService.Eliminar(
                DetalleSeleccionado.DetalleFacturaId);

            Detalles.Remove(DetalleSeleccionado);

            System.Windows.MessageBox.Show(
                "Detalle eliminado correctamente.",
                "Facturación",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo eliminar el detalle.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
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
                // ForzarError = true
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