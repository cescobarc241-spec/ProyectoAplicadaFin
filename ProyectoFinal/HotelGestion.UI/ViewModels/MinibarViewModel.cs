using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Linq;

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
    private bool _puedeEditarConsumo = true;

    public bool PuedeEditarConsumo
    {
        get => _puedeEditarConsumo;
        set => SetProperty(ref _puedeEditarConsumo, value);
    }

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

                var estancia = _estanciaService
                    .ObtenerPorId(value.EstanciaId);

                PuedeEditarConsumo =
                    estancia != null &&
                    estancia.Estado == "Activa";
            }
            else
            {
                PuedeEditarConsumo = true;
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

        set
        {
            if (!SetProperty(ref _productoMinibarId, value))
                return;

            var producto =
                Productos.FirstOrDefault(
                    p => p.ProductoMinibarId == value);

            if (producto != null)
            {
                PrecioUnitario = producto.Precio;
            }
        }
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
            if (estancia.Estado == "Activa")
            {
                Estancias.Add(estancia);
            }
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

        PuedeEditarConsumo = true;

        EstanciaId = 0;
        ProductoMinibarId = 0;
        Cantidad = 1;
        PrecioUnitario = 0;
        FechaConsumo = DateTime.Now;
    }

    private void GuardarConsumo()
    {
        if (EstanciaId <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una estancia.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (ProductoMinibarId <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar un producto.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (Cantidad <= 0)
        {
            MessageBox.Show(
                "La cantidad debe ser mayor que 0.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var producto =
            Productos.FirstOrDefault(
                p => p.ProductoMinibarId == ProductoMinibarId);

        if (producto == null)
        {
            MessageBox.Show(
                "El producto seleccionado no existe.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (producto.Estado != "Activo")
        {
            MessageBox.Show(
                "El producto seleccionado está inactivo.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (Cantidad > producto.Stock)
        {
            MessageBox.Show(
                $"Stock insuficiente. Disponible: {producto.Stock}.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        try
        {
            var consumo = new ConsumoMinibarDto
            {
                EstanciaId = EstanciaId,
                ProductoMinibarId = ProductoMinibarId,
                Cantidad = Cantidad,
                PrecioUnitario = producto.Precio,
                FechaConsumo = FechaConsumo
            };

            // Registrar el consumo
            _consumoService.Registrar(consumo);

            // Descontar el stock
            producto.Stock -= Cantidad;

            _productoService.Actualizar(producto);

            // Actualizar las listas
            CargarProductos();
            CargarConsumos();

            NuevoConsumo();

            MessageBox.Show(
                "Consumo registrado correctamente.",
                "Minibar",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "No se pudo registrar el consumo",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void EliminarConsumo()
    {
        if (ConsumoSeleccionado == null)
        {
            MessageBox.Show(
                "Seleccione un consumo.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var estancia = _estanciaService
    .ObtenerPorId(ConsumoSeleccionado.EstanciaId);

        if (estancia == null || estancia.Estado != "Activa")
        {
            MessageBox.Show(
                "La estancia ya ha sido finalizada. No se puede eliminar este consumo.",
                "Consumo no disponible",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return;
        }

        var producto =
            Productos.FirstOrDefault(
                p => p.ProductoMinibarId ==
                     ConsumoSeleccionado.ProductoMinibarId);

        int cantidad = ConsumoSeleccionado.Cantidad;

        try
        {
            // Eliminar el consumo
            _consumoService.Eliminar(
                ConsumoSeleccionado.ConsumoMinibarId);

            // Devolver la cantidad al stock
            if (producto != null)
            {
                producto.Stock += cantidad;
                _productoService.Actualizar(producto);
            }

            CargarProductos();
            CargarConsumos();

            NuevoConsumo();

            MessageBox.Show(
                "Consumo eliminado correctamente y stock restaurado.",
                "Minibar",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "No se pudo eliminar el consumo",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }
    private void ActualizarConsumo()
    {
        if (ConsumoSeleccionado == null)
        {
            MessageBox.Show(
                "Seleccione un consumo.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var estancia = _estanciaService
            .ObtenerPorId(ConsumoSeleccionado.EstanciaId);

        if (estancia == null || estancia.Estado != "Activa")
        {
            MessageBox.Show(
                "La estancia ya ha sido finalizada. No se puede modificar este consumo.",
                "Consumo no disponible",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return;
        }

        if (EstanciaId <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una estancia.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (ProductoMinibarId <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar un producto.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (Cantidad <= 0)
        {
            MessageBox.Show(
                "La cantidad debe ser mayor que 0.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var productoNuevo =
            Productos.FirstOrDefault(
                p => p.ProductoMinibarId == ProductoMinibarId);

        if (productoNuevo == null)
        {
            MessageBox.Show(
                "El producto seleccionado no existe.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (productoNuevo.Estado != "Activo")
        {
            MessageBox.Show(
                "El producto seleccionado está inactivo.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        int productoAnteriorId =
            ConsumoSeleccionado.ProductoMinibarId;

        int cantidadAnterior =
            ConsumoSeleccionado.Cantidad;

        var productoAnterior =
            Productos.FirstOrDefault(
                p => p.ProductoMinibarId == productoAnteriorId);

        try
        {
            if (productoAnteriorId == ProductoMinibarId)
            {
                int stockDisponibleReal =
                    productoNuevo.Stock + cantidadAnterior;

                if (Cantidad > stockDisponibleReal)
                {
                    MessageBox.Show(
                        $"Stock insuficiente. Disponible: {stockDisponibleReal}.",
                        "Validación",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                productoNuevo.Stock =
                    stockDisponibleReal - Cantidad;

                _productoService.Actualizar(productoNuevo);
            }
            else
            {
                if (productoAnterior != null)
                {
                    productoAnterior.Stock += cantidadAnterior;

                    _productoService.Actualizar(
                        productoAnterior);
                }

                if (Cantidad > productoNuevo.Stock)
                {
                    MessageBox.Show(
                        $"Stock insuficiente. Disponible: {productoNuevo.Stock}.",
                        "Validación",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    if (productoAnterior != null)
                    {
                        productoAnterior.Stock -= cantidadAnterior;

                        _productoService.Actualizar(
                            productoAnterior);
                    }

                    return;
                }

                productoNuevo.Stock -= Cantidad;

                _productoService.Actualizar(
                    productoNuevo);
            }

            ConsumoSeleccionado.EstanciaId = EstanciaId;
            ConsumoSeleccionado.ProductoMinibarId =
                ProductoMinibarId;
            ConsumoSeleccionado.Cantidad = Cantidad;
            ConsumoSeleccionado.PrecioUnitario =
                productoNuevo.Precio;
            ConsumoSeleccionado.FechaConsumo =
                FechaConsumo;

            _consumoService.Actualizar(
                ConsumoSeleccionado);

            CargarProductos();
            CargarConsumos();

            MessageBox.Show(
                "Consumo actualizado correctamente.",
                "Minibar",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "No se pudo actualizar el consumo",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    
    }
}