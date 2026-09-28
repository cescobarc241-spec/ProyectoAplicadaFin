using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HotelGestion.Application.Interfaces;
using HotelGestion.UI.Views;

namespace HotelGestion.UI.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private object? _vistaActual;

    private readonly IHabitacionService _habitacionService;
    private readonly ITipoHabitacionService _tipoHabitacionService;
    private readonly IReservaService _reservaService;
    private readonly IClienteService _clienteService;
    private readonly IProductoMinibarService _productoMinibarService;
    private readonly IConsumoMinibarService _consumoMinibarService;
    private readonly IEstanciaService _estanciaService;
    private readonly IFacturaService _facturaService;
    private readonly IDetalleFacturaService _detalleFacturaService;
    private readonly ICheckOutService _checkOutService;
    private readonly ICheckInService _checkInService;

    public object? VistaActual
    {
        get => _vistaActual;
        set => SetProperty(ref _vistaActual, value);
    }

    public ICommand MostrarHabitacionesCommand { get; }

    public ICommand MostrarReservasCommand { get; }

    public ICommand MostrarMinibarCommand { get; }

    public ICommand MostrarFacturacionCommand { get; }

    public ICommand MostrarTiposHabitacionCommand { get; }

    public ICommand MostrarClientesCommand { get; }
    
    public ICommand MostrarCheckInCommand { get; }
    public MainWindowViewModel(
        IHabitacionService habitacionService,
        ITipoHabitacionService tipoHabitacionService,
        IReservaService reservaService,
        IClienteService clienteService,
        IProductoMinibarService productoMinibarService,
        IConsumoMinibarService consumoMinibarService,
        IEstanciaService estanciaService,
        IFacturaService facturaService,
        IDetalleFacturaService detalleFacturaService,
        ICheckOutService checkOutService,
        ICheckInService checkInService)
    {
        _habitacionService = habitacionService;
        _tipoHabitacionService = tipoHabitacionService;
        _checkOutService = checkOutService;
        _checkInService = checkInService;
        _reservaService = reservaService;
        _clienteService = clienteService;
        _productoMinibarService = productoMinibarService;
        _consumoMinibarService = consumoMinibarService;
        _estanciaService = estanciaService;
        _facturaService = facturaService;
        _detalleFacturaService = detalleFacturaService;

        MostrarHabitacionesCommand =
            new RelayCommand(MostrarHabitaciones);

        MostrarReservasCommand =
            new RelayCommand(MostrarReservas);

        MostrarMinibarCommand =
            new RelayCommand(MostrarMinibar);

        MostrarFacturacionCommand =
            new RelayCommand(MostrarFacturacion);

        MostrarTiposHabitacionCommand =
            new RelayCommand(MostrarTiposHabitacion);

        MostrarClientesCommand = 
            new RelayCommand(MostrarClientes);

        MostrarCheckInCommand = 
            new RelayCommand(MostrarCheckIn);

        MostrarHabitaciones();
    }
    private void MostrarCheckIn()
    {
        var viewModel = new CheckInViewModel(
            _checkInService,
            _clienteService,
            _reservaService,
            _habitacionService,
            _tipoHabitacionService);

        VistaActual =
            new CheckInView
            {
                DataContext = viewModel
            };
    }
    private void MostrarClientes()
    {
        var viewModel =
            new ClienteViewModel(_clienteService);

        VistaActual =
            new ClienteView
            {
                DataContext = viewModel
            };
    }
    private void MostrarHabitaciones()
    {
        var viewModel =
            new HabitacionesViewModel(
                _habitacionService,
                _tipoHabitacionService);

        VistaActual =
            new HabitacionesView
            {
                DataContext = viewModel
            };
    }

    private void MostrarReservas()
    {
        var viewModel =
            new ReservasViewModel(
                _reservaService,
                _clienteService,
                _habitacionService,
                _tipoHabitacionService);

        VistaActual =
            new ReservasView
            {
                DataContext = viewModel
            };
    }

    private void MostrarMinibar()
    {
        var viewModel =
            new MinibarViewModel(
                _productoMinibarService,
                _consumoMinibarService,
                _estanciaService);

        VistaActual =
            new MinibarView
            {
                DataContext = viewModel
            };
    }

    private void MostrarFacturacion()
    {
        var viewModel =
            new FacturacionViewModel(
                _facturaService,
                _detalleFacturaService,
                _estanciaService,
                _checkOutService);

        VistaActual =
            new FacturacionView
            {
                DataContext = viewModel
            };
    }
    private void MostrarTiposHabitacion()
    {
        var viewModel =
            new TiposHabitacionViewModel(
                _tipoHabitacionService);

        VistaActual =
            new TiposHabitacionView
            {
                DataContext = viewModel
            };
    }
}