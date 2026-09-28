using HotelGestion.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelGestion.UI.Views
{
    /// <summary>
    /// Lógica de interacción para ReservasView.xaml
    /// </summary>
    public partial class ReservasView : UserControl
    {
        public ReservasView()
        {
            InitializeComponent();
        }

        public ReservasView(ReservasViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
