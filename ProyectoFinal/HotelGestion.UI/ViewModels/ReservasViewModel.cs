using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

namespace HotelGestion.UI.ViewModels;

public class ReservasViewModel : BaseViewModel
{
    private readonly IReservaService _reservaService;
    private readonly IClienteService _clienteService;
    private readonly IHabitacionService _habitacionService;

    private ReservaDto? _reservaSeleccionada;
    private int _clienteId;
    private int _habitacionId;
    private DateTime _fechaReserva = DateTime.Now;
    private DateTime _fechaEntrada = DateTime.Today;
    private DateTime _fechaSalida = DateTime.Today.AddDays(1);
    private int _cantidadHuespedes = 1;
    private string _estado = "Pendiente";
    private string _observaciones = string.Empty;

    public ObservableCollection<ReservaDto> Reservas { get; }
        = new();

    public ObservableCollection<ClienteDto> Clientes { get; }
        = new();

    public ObservableCollection<HabitacionDto> Habitaciones { get; }
        = new();

    public ReservaDto? ReservaSeleccionada
    {
        get => _reservaSeleccionada;
        set
        {
            if (!SetProperty(
                    ref _reservaSeleccionada,
                    value))
            {
                return;
            }

            if (value != null)
            {
                ClienteId = value.ClienteId;
                HabitacionId = value.HabitacionId;
                FechaReserva = value.FechaReserva;
                FechaEntrada = value.FechaEntrada;
                FechaSalida = value.FechaSalida;
                CantidadHuespedes = value.CantidadHuespedes;
                Estado = value.Estado;
                Observaciones = value.Observaciones ?? string.Empty;
            }
        }
    }

    public int ClienteId
    {
        get => _clienteId;
        set => SetProperty(ref _clienteId, value);
    }

    public int HabitacionId
    {
        get => _habitacionId;
        set => SetProperty(ref _habitacionId, value);
    }

    public DateTime FechaReserva
    {
        get => _fechaReserva;
        set => SetProperty(ref _fechaReserva, value);
    }

    public DateTime FechaEntrada
    {
        get => _fechaEntrada;
        set => SetProperty(ref _fechaEntrada, value);
    }

    public DateTime FechaSalida
    {
        get => _fechaSalida;
        set => SetProperty(ref _fechaSalida, value);
    }

    public int CantidadHuespedes
    {
        get => _cantidadHuespedes;
        set => SetProperty(ref _cantidadHuespedes, value);
    }

    public string Estado
    {
        get => _estado;
        set => SetProperty(ref _estado, value);
    }

    public string Observaciones
    {
        get => _observaciones;
        set => SetProperty(ref _observaciones, value);
    }

    public ICommand CargarCommand { get; }

    public ICommand NuevoCommand { get; }

    public ICommand GuardarCommand { get; }

    public ICommand ActualizarCommand { get; }

    public ICommand EliminarCommand { get; }

    public ReservasViewModel(
        IReservaService reservaService,
        IClienteService clienteService,
        IHabitacionService habitacionService)
    {
        _reservaService = reservaService;
        _clienteService = clienteService;
        _habitacionService = habitacionService;

        CargarCommand =
            new RelayCommand(CargarDatos);

        NuevoCommand =
            new RelayCommand(Nuevo);

        GuardarCommand =
            new RelayCommand(Guardar);

        ActualizarCommand =
            new RelayCommand(Actualizar);

        EliminarCommand =
            new RelayCommand(Eliminar);

        CargarDatos();
    }

    private void CargarDatos()
    {
        CargarClientes();
        CargarHabitaciones();
        CargarReservas();
    }

    private void CargarClientes()
    {
        Clientes.Clear();

        var clientes =
            _clienteService.ObtenerTodos();

        foreach (var cliente in clientes)
        {
            Clientes.Add(cliente);
        }
    }

    private void CargarHabitaciones()
    {
        Habitaciones.Clear();

        var habitaciones =
            _habitacionService.ObtenerTodos();

        foreach (var habitacion in habitaciones)
        {
            Habitaciones.Add(habitacion);
        }
    }

    private void CargarReservas()
    {
        Reservas.Clear();

        var reservas =
            _reservaService.ObtenerTodos();

        foreach (var reserva in reservas)
        {
            Reservas.Add(reserva);
        }
    }

    private void Nuevo()
    {
        ReservaSeleccionada = null;

        ClienteId = 0;
        HabitacionId = 0;
        FechaReserva = DateTime.Now;
        FechaEntrada = DateTime.Today;
        FechaSalida = DateTime.Today.AddDays(1);
        CantidadHuespedes = 1;
        Estado = "Pendiente";
        Observaciones = string.Empty;
    }

    private void Guardar()
    {
        if (ClienteId <= 0)
            return;

        if (HabitacionId <= 0)
            return;

        if (FechaSalida <= FechaEntrada)
            return;

        if (CantidadHuespedes <= 0)
            return;

        var reserva = new ReservaDto
        {
            ClienteId = ClienteId,
            HabitacionId = HabitacionId,
            FechaReserva = FechaReserva,
            FechaEntrada = FechaEntrada,
            FechaSalida = FechaSalida,
            CantidadHuespedes = CantidadHuespedes,
            Estado = Estado,
            Observaciones = Observaciones
        };

        _reservaService.Registrar(reserva);

        CargarReservas();
        Nuevo();
    }

    private void Actualizar()
    {
        if (ReservaSeleccionada == null)
            return;

        ReservaSeleccionada.ClienteId = ClienteId;
        ReservaSeleccionada.HabitacionId = HabitacionId;
        ReservaSeleccionada.FechaReserva = FechaReserva;
        ReservaSeleccionada.FechaEntrada = FechaEntrada;
        ReservaSeleccionada.FechaSalida = FechaSalida;
        ReservaSeleccionada.CantidadHuespedes = CantidadHuespedes;
        ReservaSeleccionada.Estado = Estado;
        ReservaSeleccionada.Observaciones = Observaciones;

        _reservaService.Actualizar(ReservaSeleccionada);

        CargarReservas();
    }

    private void Eliminar()
    {
        if (ReservaSeleccionada == null)
            return;

        _reservaService.Eliminar(
            ReservaSeleccionada.ReservaId);

        CargarReservas();
        Nuevo();
    }
}