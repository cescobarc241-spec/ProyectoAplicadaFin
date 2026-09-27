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

namespace HotelGestion.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var app = (App)System.Windows.Application.Current;

        var viewModel =
            new HabitacionesViewModel(app.HabitacionService);

        DataContext = viewModel;

        viewModel.CargarHabitaciones();
    }
}