using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

namespace HotelGestion.UI.ViewModels;

public class TiposHabitacionViewModel : BaseViewModel
{
    private readonly ITipoHabitacionService _tipoHabitacionService;

    private TipoHabitacionDto? _tipoSeleccionado;

    private string _nombre = string.Empty;
    private string _descripcion = string.Empty;
    private int _capacidad;
    private decimal _precioPorNoche;

    public ObservableCollection<TipoHabitacionDto> TiposHabitacion { get; }
        = new();

    public TipoHabitacionDto? TipoSeleccionado
    {
        get => _tipoSeleccionado;
        set
        {
            if (!SetProperty(
                    ref _tipoSeleccionado,
                    value))
            {
                return;
            }

            if (value != null)
            {
                Nombre = value.Nombre;
                Descripcion = value.Descripcion ?? string.Empty;
                Capacidad = value.Capacidad;
                PrecioPorNoche = value.PrecioPorNoche;
            }
        }
    }

    public string Nombre
    {
        get => _nombre;
        set => SetProperty(ref _nombre, value);
    }

    public string Descripcion
    {
        get => _descripcion;
        set => SetProperty(ref _descripcion, value);
    }

    public int Capacidad
    {
        get => _capacidad;
        set => SetProperty(ref _capacidad, value);
    }

    public decimal PrecioPorNoche
    {
        get => _precioPorNoche;
        set => SetProperty(ref _precioPorNoche, value);
    }

    public ICommand CargarCommand { get; }

    public ICommand NuevoCommand { get; }

    public ICommand GuardarCommand { get; }

    public ICommand ActualizarCommand { get; }

    public ICommand EliminarCommand { get; }

    public TiposHabitacionViewModel(
        ITipoHabitacionService tipoHabitacionService)
    {
        _tipoHabitacionService = tipoHabitacionService;

        CargarCommand =
            new RelayCommand(CargarTipos);

        NuevoCommand =
            new RelayCommand(Nuevo);

        GuardarCommand =
            new RelayCommand(Guardar);

        ActualizarCommand =
            new RelayCommand(Actualizar);

        EliminarCommand =
            new RelayCommand(Eliminar);

        CargarTipos();
    }

    private void CargarTipos()
    {
        TiposHabitacion.Clear();

        var tipos =
            _tipoHabitacionService.ObtenerTodos();

        foreach (var tipo in tipos)
        {
            TiposHabitacion.Add(tipo);
        }
    }

    private void Nuevo()
    {
        TipoSeleccionado = null;

        Nombre = string.Empty;
        Descripcion = string.Empty;
        Capacidad = 0;
        PrecioPorNoche = 0;
    }

    private void Guardar()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
            return;

        if (Capacidad <= 0)
            return;

        if (PrecioPorNoche < 0)
            return;

        var tipo = new TipoHabitacionDto
        {
            Nombre = Nombre,
            Descripcion = Descripcion,
            Capacidad = Capacidad,
            PrecioPorNoche = PrecioPorNoche
        };

        _tipoHabitacionService.Registrar(tipo);

        CargarTipos();
        Nuevo();
    }

    private void Actualizar()
    {
        if (TipoSeleccionado == null)
            return;

        TipoSeleccionado.Nombre = Nombre;
        TipoSeleccionado.Descripcion = Descripcion;
        TipoSeleccionado.Capacidad = Capacidad;
        TipoSeleccionado.PrecioPorNoche = PrecioPorNoche;

        _tipoHabitacionService.Actualizar(TipoSeleccionado);

        CargarTipos();
    }

    private void Eliminar()
    {
        if (TipoSeleccionado == null)
            return;

        _tipoHabitacionService.Eliminar(
            TipoSeleccionado.TipoHabitacionId);

        CargarTipos();
        Nuevo();
    }
}
