    using HotelGestion.Application.DTOs;
    using HotelGestion.Application.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
using System.Windows;
    using System.Windows.Input;

    namespace HotelGestion.UI.ViewModels;

    public class ReservasViewModel : BaseViewModel
    {
        private readonly IReservaService _reservaService;
        private readonly IClienteService _clienteService;
        private readonly IHabitacionService _habitacionService;
    private readonly ITipoHabitacionService _tipoHabitacionService;

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

        public ObservableCollection<HabitacionMostrar> Habitaciones { get; set; } 
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
        public class HabitacionMostrar
        {
            public int HabitacionId { get; set; }
            public string Numero { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;

            public string NombreCompleto =>
                $"{Numero} - {Tipo}";
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
            IHabitacionService habitacionService,
            ITipoHabitacionService tipoHabitacionService)
        {
            _reservaService = reservaService;
            _clienteService = clienteService;
            _habitacionService = habitacionService;
            _tipoHabitacionService = tipoHabitacionService;

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

            var habitaciones = _habitacionService
                .ObtenerTodos()
                .Where(h => h.Estado == "Disponible")
                .ToList();

            var tipos = _tipoHabitacionService
                .ObtenerTodos()
                .ToList();

            foreach (var habitacion in habitaciones)
            {
                var tipo = tipos.FirstOrDefault(
                    t => t.TipoHabitacionId == habitacion.TipoHabitacionId);

                Habitaciones.Add(new HabitacionMostrar
                {
                    HabitacionId = habitacion.HabitacionId,
                    Numero = habitacion.Numero,
                    Tipo = tipo?.Nombre ?? string.Empty
                });
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
            {
                MessageBox.Show(
                    "Debe seleccionar un cliente.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (HabitacionId <= 0)
            {
                MessageBox.Show(
                    "Debe seleccionar una habitación.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (FechaEntrada.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "La fecha de entrada no puede ser anterior a hoy.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (FechaSalida.Date <= FechaEntrada.Date)
            {
                MessageBox.Show(
                    "La fecha de salida debe ser posterior a la fecha de entrada.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (CantidadHuespedes <= 0)
            {
                MessageBox.Show(
                    "La cantidad de huéspedes debe ser mayor que 0.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            try
            {
                var reserva = new ReservaDto
                {
                    ClienteId = ClienteId,
                    HabitacionId = HabitacionId,
                    FechaReserva = FechaReserva,
                    FechaEntrada = FechaEntrada,
                    FechaSalida = FechaSalida,
                    CantidadHuespedes = CantidadHuespedes,
                    Estado = Estado,
                    Observaciones = string.IsNullOrWhiteSpace(Observaciones)
                        ? null
                        : Observaciones
                };

                _reservaService.Registrar(reserva);

                var habitacion =
                    _habitacionService.ObtenerPorId(HabitacionId);

                if (habitacion != null)
                {
                    habitacion.Estado = "Reservada";
                    _habitacionService.Actualizar(habitacion);
                }

                CargarReservas();
                CargarHabitaciones();
                Nuevo();

                MessageBox.Show(
                    "Reserva registrada correctamente.",
                    "Reservas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo registrar la reserva",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void Actualizar()
        {
            if (ReservaSeleccionada == null)
            {
                MessageBox.Show(
                    "Seleccione una reserva.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (ClienteId <= 0)
            {
                MessageBox.Show(
                    "Debe seleccionar un cliente.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (HabitacionId <= 0)
            {
                MessageBox.Show(
                    "Debe seleccionar una habitación.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (FechaSalida.Date <= FechaEntrada.Date)
            {
                MessageBox.Show(
                    "La fecha de salida debe ser posterior a la fecha de entrada.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (CantidadHuespedes <= 0)
            {
                MessageBox.Show(
                    "La cantidad de huéspedes debe ser mayor que 0.",
                    "Validación",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            try
            {
                int habitacionAnteriorId =
                    ReservaSeleccionada.HabitacionId;

                ReservaSeleccionada.ClienteId = ClienteId;
                ReservaSeleccionada.HabitacionId = HabitacionId;
                ReservaSeleccionada.FechaReserva = FechaReserva;
                ReservaSeleccionada.FechaEntrada = FechaEntrada;
                ReservaSeleccionada.FechaSalida = FechaSalida;
                ReservaSeleccionada.CantidadHuespedes = CantidadHuespedes;
                ReservaSeleccionada.Estado = Estado;
                ReservaSeleccionada.Observaciones =
                    string.IsNullOrWhiteSpace(Observaciones)
                        ? null
                        : Observaciones;

                _reservaService.Actualizar(ReservaSeleccionada);

                if (habitacionAnteriorId != HabitacionId)
                {
                    var habitacionAnterior =
                        _habitacionService.ObtenerPorId(habitacionAnteriorId);

                    if (habitacionAnterior != null &&
                        habitacionAnterior.Estado == "Reservada")
                    {
                        habitacionAnterior.Estado = "Disponible";
                        _habitacionService.Actualizar(habitacionAnterior);
                    }

                    var nuevaHabitacion =
                        _habitacionService.ObtenerPorId(HabitacionId);

                    if (nuevaHabitacion != null)
                    {
                        nuevaHabitacion.Estado = "Reservada";
                        _habitacionService.Actualizar(nuevaHabitacion);
                    }
                }

                if (Estado == "Cancelada")
                {
                    var habitacion =
                        _habitacionService.ObtenerPorId(HabitacionId);

                    if (habitacion != null &&
                        habitacion.Estado == "Reservada")
                    {
                        habitacion.Estado = "Disponible";
                        _habitacionService.Actualizar(habitacion);
                    }
                }

                CargarReservas();
                CargarHabitaciones();

                MessageBox.Show(
                    "Reserva actualizada correctamente.",
                    "Reservas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo actualizar la reserva",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
        private void Eliminar()
        {
            if (ReservaSeleccionada == null)
            {
                MessageBox.Show(
                    "Debe seleccionar una reserva.",
                    "Reservas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // No permitir eliminar reservas completadas
            if (ReservaSeleccionada.Estado == "Completada")
            {
                MessageBox.Show(
                    "Esta reserva ya ha sido completada y forma parte del historial del huésped.\n\n" +
                    "No se puede eliminar una reserva completada.",
                    "Reserva no disponible para eliminar",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            var respuesta = MessageBox.Show(
                "¿Está seguro de eliminar la reserva seleccionada?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (respuesta != MessageBoxResult.Yes)
                return;

            try
            {
                _reservaService.Eliminar(ReservaSeleccionada.ReservaId);

                CargarDatos();

                ReservaSeleccionada = null;

                MessageBox.Show(
                    "La reserva se eliminó correctamente.",
                    "Reservas",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo eliminar la reserva.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }      