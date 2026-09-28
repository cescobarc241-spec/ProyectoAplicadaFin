using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

namespace HotelGestion.UI.ViewModels;

public class ClienteViewModel : BaseViewModel
{
    private readonly IClienteService _clienteService;

    public ObservableCollection<ClienteDto> Clientes { get; } = new();

    // =========================================================
    // CLIENTE SELECCIONADO
    // =========================================================

    private ClienteDto? _clienteSeleccionado;

    public ClienteDto? ClienteSeleccionado
    {
        get => _clienteSeleccionado;

        set
        {
            if (!SetProperty(ref _clienteSeleccionado, value))
                return;

            if (_clienteSeleccionado != null)
            {
                ClienteId = _clienteSeleccionado.ClienteId;
                TipoDocumento = _clienteSeleccionado.TipoDocumento;
                NumeroDocumento = _clienteSeleccionado.NumeroDocumento;
                Nombres = _clienteSeleccionado.Nombres;
                Apellidos = _clienteSeleccionado.Apellidos;
                Telefono = _clienteSeleccionado.Telefono ?? string.Empty;
                Correo = _clienteSeleccionado.Correo ?? string.Empty;
                Direccion = _clienteSeleccionado.Direccion ?? string.Empty;
            }
        }
    }

    // =========================================================
    // CAMPOS
    // =========================================================

    private int _clienteId;

    public int ClienteId
    {
        get => _clienteId;
        set => SetProperty(ref _clienteId, value);
    }

    private string _tipoDocumento = "DNI";

    public string TipoDocumento
    {
        get => _tipoDocumento;
        set => SetProperty(ref _tipoDocumento, value);
    }

    private string _numeroDocumento = string.Empty;

    public string NumeroDocumento
    {
        get => _numeroDocumento;
        set => SetProperty(ref _numeroDocumento, value);
    }

    private string _nombres = string.Empty;

    public string Nombres
    {
        get => _nombres;
        set => SetProperty(ref _nombres, value);
    }

    private string _apellidos = string.Empty;

    public string Apellidos
    {
        get => _apellidos;
        set => SetProperty(ref _apellidos, value);
    }

    private string _telefono = string.Empty;

    public string Telefono
    {
        get => _telefono;
        set => SetProperty(ref _telefono, value);
    }

    private string _correo = string.Empty;

    public string Correo
    {
        get => _correo;
        set => SetProperty(ref _correo, value);
    }

    private string _direccion = string.Empty;

    public string Direccion
    {
        get => _direccion;
        set => SetProperty(ref _direccion, value);
    }

    // =========================================================
    // COMANDOS
    // =========================================================

    public RelayCommand CargarCommand { get; }

    public RelayCommand NuevoCommand { get; }

    public RelayCommand GuardarCommand { get; }

    public RelayCommand ActualizarCommand { get; }

    public RelayCommand EliminarCommand { get; }

    // =========================================================
    // CONSTRUCTOR
    // =========================================================

    public ClienteViewModel(IClienteService clienteService)
    {
        _clienteService = clienteService;

        CargarCommand =
            new RelayCommand(CargarClientes);

        NuevoCommand =
            new RelayCommand(NuevoCliente);

        GuardarCommand =
            new RelayCommand(GuardarCliente);

        ActualizarCommand =
            new RelayCommand(ActualizarCliente);

        EliminarCommand =
            new RelayCommand(EliminarCliente);

        CargarClientes();
    }

    // =========================================================
    // CARGAR
    // =========================================================

    private void CargarClientes()
    {
        Clientes.Clear();

        var lista = _clienteService.ObtenerTodos();

        foreach (var cliente in lista)
        {
            Clientes.Add(cliente);
        }
    }

    // =========================================================
    // NUEVO
    // =========================================================

    private void NuevoCliente()
    {
        ClienteSeleccionado = null;

        ClienteId = 0;
        TipoDocumento = "DNI";
        NumeroDocumento = string.Empty;
        Nombres = string.Empty;
        Apellidos = string.Empty;
        Telefono = string.Empty;
        Correo = string.Empty;
        Direccion = string.Empty;
    }

    // =========================================================
    // GUARDAR
    // =========================================================

    private void GuardarCliente()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(NumeroDocumento))
            {
                System.Windows.MessageBox.Show(
                    "Debe ingresar el número de documento.",
                    "Clientes",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(Nombres))
            {
                System.Windows.MessageBox.Show(
                    "Debe ingresar los nombres.",
                    "Clientes",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(Apellidos))
            {
                System.Windows.MessageBox.Show(
                    "Debe ingresar los apellidos.",
                    "Clientes",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            var cliente = new ClienteDto
            {
                TipoDocumento = TipoDocumento,
                NumeroDocumento = NumeroDocumento,
                Nombres = Nombres,
                Apellidos = Apellidos,
                Telefono = string.IsNullOrWhiteSpace(Telefono)
                    ? null
                    : Telefono,
                Correo = string.IsNullOrWhiteSpace(Correo)
                    ? null
                    : Correo,
                Direccion = string.IsNullOrWhiteSpace(Direccion)
                    ? null
                    : Direccion
            };

            int id = _clienteService.Registrar(cliente);

            cliente.ClienteId = id;

            Clientes.Add(cliente);

            ClienteSeleccionado = cliente;

            System.Windows.MessageBox.Show(
                "Cliente registrado correctamente.",
                "Clientes",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo registrar el cliente.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    // =========================================================
    // ACTUALIZAR
    // =========================================================

    private void ActualizarCliente()
    {
        try
        {
            if (ClienteSeleccionado == null)
            {
                System.Windows.MessageBox.Show(
                    "Seleccione un cliente.",
                    "Clientes",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            ClienteSeleccionado.TipoDocumento = TipoDocumento;
            ClienteSeleccionado.NumeroDocumento = NumeroDocumento;
            ClienteSeleccionado.Nombres = Nombres;
            ClienteSeleccionado.Apellidos = Apellidos;
            ClienteSeleccionado.Telefono =
                string.IsNullOrWhiteSpace(Telefono)
                    ? null
                    : Telefono;
            ClienteSeleccionado.Correo =
                string.IsNullOrWhiteSpace(Correo)
                    ? null
                    : Correo;
            ClienteSeleccionado.Direccion =
                string.IsNullOrWhiteSpace(Direccion)
                    ? null
                    : Direccion;

            _clienteService.Actualizar(
                ClienteSeleccionado);

            CargarClientes();

            System.Windows.MessageBox.Show(
                "Cliente actualizado correctamente.",
                "Clientes",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo actualizar el cliente.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    // =========================================================
    // ELIMINAR
    // =========================================================

    private void EliminarCliente()
    {
        try
        {
            if (ClienteSeleccionado == null)
            {
                System.Windows.MessageBox.Show(
                    "Seleccione un cliente.",
                    "Clientes",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                return;
            }

            _clienteService.Eliminar(
                ClienteSeleccionado.ClienteId);

            Clientes.Remove(ClienteSeleccionado);

            ClienteSeleccionado = null;

            NuevoCliente();

            System.Windows.MessageBox.Show(
                "Cliente eliminado correctamente.",
                "Clientes",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                "No se pudo eliminar el cliente.\n\n" +
                ex.Message,
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }
}