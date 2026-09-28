using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

namespace HotelGestion.UI.ViewModels;

public class MinibarViewModel : BaseViewModel
{
    private readonly IProductoMinibarService _productoService;
    private readonly IConsumoMinibarService _consumoService;
    private readonly IEstanciaService _estanciaService;

    private ProductoMinibarDto? _productoSeleccionado;
    private ConsumoMinibarDto? _consumoSeleccionado;

    private string _nombreProducto = string.Empty;
    private string _descripcionProducto = string.Empty;
    private decimal _precioProducto;
    private int _stockProducto;
    private string _estadoProducto = "Activo";

    private int _estanciaId;
    private int _productoMinibarId;
    private int _cantidad = 1;
    private decimal _precioUnitario;
    private DateTime _fechaConsumo = DateTime.Now;

    public ObservableCollection<ProductoMinibarDto> Productos { get; }
        = new();

    public ObservableCollection<ConsumoMinibarDto> Consumos { get; }
        = new();

    public ObservableCollection<EstanciaDto> Estancias { get; }
        = new();

    public ProductoMinibarDto? ProductoSeleccionado
    {
        get => _productoSeleccionado;
        set
        {
            if (!SetProperty(
                    ref _productoSeleccionado,
                    value))
            {
                return;
            }

            if (value != null)
            {
                NombreProducto = value.Nombre;
                DescripcionProducto = value.Descripcion ?? string.Empty;
                PrecioProducto = value.Precio;
                StockProducto = value.Stock;
                EstadoProducto = value.Estado;
            }
        }
    }

    public ConsumoMinibarDto? ConsumoSeleccionado
    {
        get => _consumoSeleccionado;
        set
        {
            if (!SetProperty(
                    ref _consumoSeleccionado,
                    value))
            {
                return;
            }

            if (value != null)
            {
                EstanciaId = value.EstanciaId;
                ProductoMinibarId = value.ProductoMinibarId;
                Cantidad = value.Cantidad;
                PrecioUnitario = value.PrecioUnitario;
                FechaConsumo = value.FechaConsumo;
            }
        }
    }

    public string NombreProducto
    {
        get => _nombreProducto;
        set => SetProperty(ref _nombreProducto, value);
    }

    public string DescripcionProducto
    {
        get => _descripcionProducto;
        set => SetProperty(ref _descripcionProducto, value);
    }

    public decimal PrecioProducto
    {
        get => _precioProducto;
        set => SetProperty(ref _precioProducto, value);
    }

    public int StockProducto
    {
        get => _stockProducto;
        set => SetProperty(ref _stockProducto, value);
    }

    public string EstadoProducto
    {
        get => _estadoProducto;
        set => SetProperty(ref _estadoProducto, value);
    }

    public int EstanciaId
    {
        get => _estanciaId;
        set => SetProperty(ref _estanciaId, value);
    }

    public int ProductoMinibarId
    {
        get => _productoMinibarId;
        set => SetProperty(ref _productoMinibarId, value);
    }

    public int Cantidad
    {
        get => _cantidad;
        set => SetProperty(ref _cantidad, value);
    }

    public decimal PrecioUnitario
    {
        get => _precioUnitario;
        set => SetProperty(ref _precioUnitario, value);
    }

    public DateTime FechaConsumo
    {
        get => _fechaConsumo;
        set => SetProperty(ref _fechaConsumo, value);
    }

    public ICommand CargarCommand { get; }

    public ICommand NuevoProductoCommand { get; }

    public ICommand GuardarProductoCommand { get; }

    public ICommand ActualizarProductoCommand { get; }

    public ICommand EliminarProductoCommand { get; }

    public ICommand NuevoConsumoCommand { get; }

    public ICommand GuardarConsumoCommand { get; }

    public ICommand ActualizarConsumoCommand { get; }

    public ICommand EliminarConsumoCommand { get; }

    public MinibarViewModel(
        IProductoMinibarService productoService,
        IConsumoMinibarService consumoService,
        IEstanciaService estanciaService)
    {
        _productoService = productoService;
        _consumoService = consumoService;
        _estanciaService = estanciaService;

        CargarCommand =
            new RelayCommand(CargarDatos);

        NuevoProductoCommand =
            new RelayCommand(NuevoProducto);

        GuardarProductoCommand =
            new RelayCommand(GuardarProducto);

        ActualizarProductoCommand =
            new RelayCommand(ActualizarProducto);

        EliminarProductoCommand =
            new RelayCommand(EliminarProducto);

        NuevoConsumoCommand =
            new RelayCommand(NuevoConsumo);

        GuardarConsumoCommand =
            new RelayCommand(GuardarConsumo);

        ActualizarConsumoCommand =
            new RelayCommand(ActualizarConsumo);

        EliminarConsumoCommand =
            new RelayCommand(EliminarConsumo);

        CargarDatos();
    }

    private void CargarDatos()
    {
        CargarProductos();
        CargarEstancias();
        CargarConsumos();
    }

    private void CargarProductos()
    {
        Productos.Clear();

        var productos =
            _productoService.ObtenerTodos();

        foreach (var producto in productos)
        {
            Productos.Add(producto);
        }
    }

    private void CargarEstancias()
    {
        Estancias.Clear();

        var estancias =
            _estanciaService.ObtenerTodos();

        foreach (var estancia in estancias)
        {
            Estancias.Add(estancia);
        }
    }

    private void CargarConsumos()
    {
        Consumos.Clear();

        var consumos =
            _consumoService.ObtenerTodos();

        foreach (var consumo in consumos)
        {
            Consumos.Add(consumo);
        }
    }

    private void NuevoProducto()
    {
        ProductoSeleccionado = null;

        NombreProducto = string.Empty;
        DescripcionProducto = string.Empty;
        PrecioProducto = 0;
        StockProducto = 0;
        EstadoProducto = "Activo";
    }

    private void GuardarProducto()
    {
        if (string.IsNullOrWhiteSpace(NombreProducto))
            return;

        if (PrecioProducto < 0)
            return;

        if (StockProducto < 0)
            return;

        var producto = new ProductoMinibarDto
        {
            Nombre = NombreProducto,
            Descripcion = DescripcionProducto,
            Precio = PrecioProducto,
            Stock = StockProducto,
            Estado = EstadoProducto
        };

        _productoService.Registrar(producto);

        CargarProductos();
        NuevoProducto();
    }

    private void ActualizarProducto()
    {
        if (ProductoSeleccionado == null)
            return;

        ProductoSeleccionado.Nombre = NombreProducto;
        ProductoSeleccionado.Descripcion = DescripcionProducto;
        ProductoSeleccionado.Precio = PrecioProducto;
        ProductoSeleccionado.Stock = StockProducto;
        ProductoSeleccionado.Estado = EstadoProducto;

        _productoService.Actualizar(ProductoSeleccionado);

        CargarProductos();
    }

    private void EliminarProducto()
    {
        if (ProductoSeleccionado == null)
            return;

        _productoService.Eliminar(
            ProductoSeleccionado.ProductoMinibarId);

        CargarProductos();
        NuevoProducto();
    }

    private void NuevoConsumo()
    {
        ConsumoSeleccionado = null;

        EstanciaId = 0;
        ProductoMinibarId = 0;
        Cantidad = 1;
        PrecioUnitario = 0;
        FechaConsumo = DateTime.Now;
    }

    private void GuardarConsumo()
    {
        if (EstanciaId <= 0)
            return;

        if (ProductoMinibarId <= 0)
            return;

        if (Cantidad <= 0)
            return;

        if (PrecioUnitario < 0)
            return;

        var consumo = new ConsumoMinibarDto
        {
            EstanciaId = EstanciaId,
            ProductoMinibarId = ProductoMinibarId,
            Cantidad = Cantidad,
            PrecioUnitario = PrecioUnitario,
            FechaConsumo = FechaConsumo
        };

        _consumoService.Registrar(consumo);

        CargarConsumos();
        NuevoConsumo();
    }

    private void ActualizarConsumo()
    {
        if (ConsumoSeleccionado == null)
            return;

        ConsumoSeleccionado.EstanciaId = EstanciaId;
        ConsumoSeleccionado.ProductoMinibarId = ProductoMinibarId;
        ConsumoSeleccionado.Cantidad = Cantidad;
        ConsumoSeleccionado.PrecioUnitario = PrecioUnitario;
        ConsumoSeleccionado.FechaConsumo = FechaConsumo;

        _consumoService.Actualizar(ConsumoSeleccionado);

        CargarConsumos();
    }

    private void EliminarConsumo()
    {
        if (ConsumoSeleccionado == null)
            return;

        _consumoService.Eliminar(
            ConsumoSeleccionado.ConsumoMinibarId);

        CargarConsumos();
        NuevoConsumo();
    }
}