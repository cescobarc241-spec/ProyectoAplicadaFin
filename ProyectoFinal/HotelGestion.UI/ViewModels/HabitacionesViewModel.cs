using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

namespace HotelGestion.UI.ViewModels;

public class HabitacionesViewModel : BaseViewModel
{
    private readonly IHabitacionService _habitacionService;
    private readonly ITipoHabitacionService _tipoHabitacionService;

    private HabitacionDto? _habitacionSeleccionada;

    private string _numero = string.Empty;
    private int _piso;
    private int _tipoHabitacionId;
    private string _estado = "Disponible";
    private string _descripcion = string.Empty;

    public ObservableCollection<HabitacionDto> Habitaciones { get; }
        = new();

    public ObservableCollection<TipoHabitacionDto> TiposHabitacion { get; }
        = new();

    public HabitacionDto? HabitacionSeleccionada
    {
        get => _habitacionSeleccionada;
        set
        {
            if (!SetProperty(
                    ref _habitacionSeleccionada,
                    value))
            {
                return;
            }

            if (value != null)
            {
                Numero = value.Numero;
                Piso = value.Piso;
                TipoHabitacionId = value.TipoHabitacionId;
                Estado = value.Estado;
                Descripcion = value.Descripcion ?? string.Empty;
            }
        }
    }

    public string Numero
    {
        get => _numero;
        set => SetProperty(ref _numero, value);
    }

    public int Piso
    {
        get => _piso;
        set => SetProperty(ref _piso, value);
    }

    public int TipoHabitacionId
    {
        get => _tipoHabitacionId;
        set => SetProperty(ref _tipoHabitacionId, value);
    }

    public string Estado
    {
        get => _estado;
        set => SetProperty(ref _estado, value);
    }

    public string Descripcion
    {
        get => _descripcion;
        set => SetProperty(ref _descripcion, value);
    }

    public ICommand CargarCommand { get; }

    public ICommand NuevoCommand { get; }

    public ICommand GuardarCommand { get; }

    public ICommand ActualizarCommand { get; }

    public ICommand EliminarCommand { get; }

    public HabitacionesViewModel(
        IHabitacionService habitacionService,
        ITipoHabitacionService tipoHabitacionService)
    {
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
        CargarTiposHabitacion();
        CargarHabitaciones();
    }

    private void CargarTiposHabitacion()
    {
        TiposHabitacion.Clear();

        var tipos =
            _tipoHabitacionService.ObtenerTodos();

        foreach (var tipo in tipos)
        {
            TiposHabitacion.Add(tipo);
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

    private void Nuevo()
    {
        HabitacionSeleccionada = null;

        Numero = string.Empty;
        Piso = 0;
        TipoHabitacionId = 0;
        Estado = "Disponible";
        Descripcion = string.Empty;
    }

    private void Guardar()
    {
        if (string.IsNullOrWhiteSpace(Numero))
            return;

        if (Piso <= 0)
            return;

        if (TipoHabitacionId <= 0)
            return;

        var habitacion = new HabitacionDto
        {
            Numero = Numero,
            Piso = Piso,
            TipoHabitacionId = TipoHabitacionId,
            Estado = Estado,
            Descripcion = Descripcion
        };

        _habitacionService.Registrar(habitacion);

        CargarHabitaciones();
        Nuevo();
    }

    private void Actualizar()
    {
        if (HabitacionSeleccionada == null)
            return;

        HabitacionSeleccionada.Numero = Numero;
        HabitacionSeleccionada.Piso = Piso;
        HabitacionSeleccionada.TipoHabitacionId = TipoHabitacionId;
        HabitacionSeleccionada.Estado = Estado;
        HabitacionSeleccionada.Descripcion = Descripcion;

        _habitacionService.Actualizar(HabitacionSeleccionada);

        CargarHabitaciones();
    }

    private void Eliminar()
    {
        if (HabitacionSeleccionada == null)
            return;

        _habitacionService.Eliminar(
            HabitacionSeleccionada.HabitacionId);

        CargarHabitaciones();
        Nuevo();
    }
}