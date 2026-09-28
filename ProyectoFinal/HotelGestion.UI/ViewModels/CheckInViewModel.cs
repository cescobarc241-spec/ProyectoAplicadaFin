using System;
using System.Collections.Generic;
using System.Text;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HotelGestion.UI.ViewModels;

public class CheckInViewModel : BaseViewModel
{
    private readonly ICheckInService _checkInService;
    private readonly IClienteService _clienteService;
    private readonly IReservaService _reservaService;
    private readonly IHabitacionService _habitacionService;
    private readonly ITipoHabitacionService _tipoHabitacionService;

    // =========================================================
    // COLECCIONES
    // =========================================================

    public ObservableCollection<ClienteDto> Clientes { get; } = new();

    public ObservableCollection<ReservaDto> Reservas { get; } = new(); 
    public ObservableCollection<HabitacionMostrar> Habitaciones { get; set; } = new();

    // =========================================================
    // SELECCIONES
    // =========================================================
    private HabitacionMostrar? _habitacionSeleccionada;

    public HabitacionMostrar? HabitacionSeleccionada
    {
        get => _habitacionSeleccionada;
        set
        {
            if (!SetProperty(ref _habitacionSeleccionada, value))
                return;

            if (value != null)
                HabitacionId = value.HabitacionId;
        }
    }
    private ReservaDto? _reservaSeleccionada;

    public ReservaDto? ReservaSeleccionada
    {
        get => _reservaSeleccionada;

        set
        {
            if (!SetProperty(ref _reservaSeleccionada, value))
                return;

            if (value != null)
            {
                ClienteId = value.ClienteId;
                HabitacionId = value.HabitacionId;
                FechaSalida = value.FechaSalida;
                CantidadHuespedes = value.CantidadHuespedes;
                Observaciones = value.Observaciones;

                CargarHabitaciones();
            }
            else
            {
                HabitacionSeleccionada = null;
                CargarHabitaciones();
            }
        }
    }
    // =========================================================
    // DATOS DEL CHECK-IN
    // =========================================================

    private bool _conReserva = true;
    public bool PuedeSeleccionarHabitacion => !ConReserva;

    public bool ConReserva
    {
        get => _conReserva;

        set
        {
            if (!SetProperty(ref _conReserva, value))
                return;
            OnPropertyChanged(nameof(PuedeSeleccionarHabitacion));

            if (value)
            {
                CargarReservas();
            }
            else
            {
                ReservaSeleccionada = null;
                ClienteId = 0;
                HabitacionId = 0;
                FechaSalida = DateTime.Today.AddDays(1);
                CantidadHuespedes = 1;
                Observaciones = string.Empty;

                CargarHabitaciones();
            }

            EjecutarCheckInCommand.RaiseCanExecuteChanged();
        }
    }

    private int _clienteId;

    public int ClienteId
    {
        get => _clienteId;
        set => SetProperty(ref _clienteId, value);
    }

    private int _reservaId;

    public int ReservaId
    {
        get => _reservaId;
        set => SetProperty(ref _reservaId, value);
    }

    private int _habitacionId;

    public int HabitacionId
    {
        get => _habitacionId;
        set => SetProperty(ref _habitacionId, value);
    }

    private DateTime _fechaCheckIn = DateTime.Now;

    public DateTime FechaCheckIn
    {
        get => _fechaCheckIn;
        set => SetProperty(ref _fechaCheckIn, value);
    }

    private DateTime _fechaSalida = DateTime.Today.AddDays(1);

    public DateTime FechaSalida
    {
        get => _fechaSalida;
        set => SetProperty(ref _fechaSalida, value);
    }

    private int _cantidadHuespedes = 1;

    public int CantidadHuespedes
    {
        get => _cantidadHuespedes;
        set => SetProperty(ref _cantidadHuespedes, value);
    }

    private string _observaciones = string.Empty;

    public string Observaciones
    {
        get => _observaciones;
        set => SetProperty(ref _observaciones, value);
    }

    // =========================================================
    // COMANDO
    // =========================================================

    public RelayCommand EjecutarCheckInCommand { get; }

    public RelayCommand CargarCommand { get; }

    // =========================================================
    // CONSTRUCTOR
    // =========================================================

    public CheckInViewModel(
    ICheckInService checkInService,
    IClienteService clienteService,
    IReservaService reservaService,
    IHabitacionService habitacionService,
    ITipoHabitacionService tipoHabitacionService)
    {
        _checkInService = checkInService;
        _clienteService = clienteService;
        _reservaService = reservaService;
        _habitacionService = habitacionService;
        _tipoHabitacionService = tipoHabitacionService;

        Clientes = new ObservableCollection<ClienteDto>();
        Reservas = new ObservableCollection<ReservaDto>();
        Habitaciones = new ObservableCollection<HabitacionMostrar>();

        CargarCommand = new RelayCommand(CargarDatos);
        EjecutarCheckInCommand = new RelayCommand(EjecutarCheckIn);

        CargarDatos();
    }
    public class HabitacionMostrar
    {
        public int HabitacionId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

        public string NombreCompleto =>
            $"{Numero} - {Tipo}";
    }
    // =========================================================
    // CARGAR DATOS
    // =========================================================

    private void CargarDatos()
    {
        CargarClientes();
        CargarReservas();
        CargarHabitaciones();
    }

    private void CargarClientes()
    {
        Clientes.Clear();

        foreach (var cliente in _clienteService.ObtenerTodos())
        {
            Clientes.Add(cliente);
        }
    }

    private void CargarReservas()
    {
        Reservas.Clear();

        foreach (var reserva in _reservaService.ObtenerTodos())
        {
            if (reserva.Estado == "Confirmada")
            {
                Reservas.Add(reserva);
            }
        }
    }

    private void CargarHabitaciones()
    {
        Habitaciones.Clear();

        var todasLasHabitaciones = _habitacionService
            .ObtenerTodos()
            .ToList();

        var tipos = _tipoHabitacionService
            .ObtenerTodos()
            .ToList();

        // ============================================
        // SI HAY UNA RESERVA SELECCIONADA
        // ============================================
        if (ReservaSeleccionada != null)
        {
            var habitacionReservada = todasLasHabitaciones
                .FirstOrDefault(h =>
                    h.HabitacionId == ReservaSeleccionada.HabitacionId);

            if (habitacionReservada != null)
            {
                var tipo = tipos.FirstOrDefault(
                    t => t.TipoHabitacionId ==
                         habitacionReservada.TipoHabitacionId);

                Habitaciones.Add(new HabitacionMostrar
                {
                    HabitacionId = habitacionReservada.HabitacionId,
                    Numero = habitacionReservada.Numero,
                    Tipo = tipo?.Nombre ?? "Sin tipo"
                });

                // Seleccionar automáticamente
                HabitacionSeleccionada = Habitaciones.First();
            }

            return;
        }

        // ============================================
        // SIN RESERVA
        // ============================================
        var habitacionesDisponibles = todasLasHabitaciones
            .Where(h => h.Estado == "Disponible")
            .ToList();

        foreach (var habitacion in habitacionesDisponibles)
        {
            var tipo = tipos.FirstOrDefault(
                t => t.TipoHabitacionId == habitacion.TipoHabitacionId);

            Habitaciones.Add(new HabitacionMostrar
            {
                HabitacionId = habitacion.HabitacionId,
                Numero = habitacion.Numero,
                Tipo = tipo?.Nombre ?? "Sin tipo"
            });
        }
    }

    // =========================================================
    // EJECUTAR CHECK-IN
    // =========================================================

    private void EjecutarCheckIn()
    {
        try
        {
            if (ClienteId <= 0)
            {
                MessageBox.Show(
                    "Debe seleccionar un cliente.",
                    "Check-In",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (HabitacionId <= 0)
            {
                MessageBox.Show(
                    "Debe seleccionar una habitación.",
                    "Check-In",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (FechaSalida.Date <= FechaCheckIn.Date)
            {
                MessageBox.Show(
                    "La fecha de salida debe ser posterior al check-in.",
                    "Check-In",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (CantidadHuespedes <= 0)
            {
                MessageBox.Show(
                    "La cantidad de huéspedes debe ser mayor que cero.",
                    "Check-In",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (ConReserva && ReservaSeleccionada == null)
            {
                MessageBox.Show(
                    "Debe seleccionar una reserva.",
                    "Check-In",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var checkIn = new CheckInDto
            {
                ClienteId = ClienteId,
                ReservaId = ConReserva
                    ? ReservaSeleccionada!.ReservaId
                    : 0,
                HabitacionId = HabitacionId,
                FechaCheckIn = FechaCheckIn,
                FechaSalida = FechaSalida,
                CantidadHuespedes = CantidadHuespedes,
                Observaciones = Observaciones,
                ConReserva = ConReserva
            };

            int estanciaId =
                _checkInService.Ejecutar(checkIn);

            MessageBox.Show(
                $"Check-In realizado correctamente.\n\nEstancia creada: {estanciaId}",
                "Check-In",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            CargarDatos();
            NuevoCheckIn();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Error en Check-In",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }


    // =========================================================
    // NUEVO CHECK-IN
    // =========================================================

    public void NuevoCheckIn()
    {
        ReservaSeleccionada = null;

        ClienteId = 0;
        HabitacionId = 0;
        ReservaId = 0;

        FechaCheckIn = DateTime.Now;
        FechaSalida = DateTime.Today.AddDays(1);

        CantidadHuespedes = 1;
        Observaciones = string.Empty;

        CargarHabitaciones();
    }
}