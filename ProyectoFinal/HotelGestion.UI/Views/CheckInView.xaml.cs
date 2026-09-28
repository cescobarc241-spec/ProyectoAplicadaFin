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
using HotelGestion.UI.ViewModels;

namespace HotelGestion.UI.Views;

public partial class CheckInView : UserControl
{
    public CheckInView()
    {
        InitializeComponent();
    }

    private void NuevoCheckIn_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is CheckInViewModel viewModel)
        {
            viewModel.NuevoCheckIn();
        }
    }
}