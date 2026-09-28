using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HotelGestion.UI.ViewModels;
using HotelGestion.Application.Interfaces;
namespace HotelGestion.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var app =
            (App)System.Windows.Application.Current;

        DataContext =
            new MainWindowViewModel(
                app.HabitacionService,
                app.TipoHabitacionService,
                app.ReservaService,
                app.ClienteService,
                app.ProductoMinibarService,
                app.ConsumoMinibarService,
                app.EstanciaService,
                app.FacturaService,
                app.DetalleFacturaService,
                app.CheckOutService,
                app.CheckInService);
    }
}