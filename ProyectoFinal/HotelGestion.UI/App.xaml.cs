using HotelGestion.Infrastructure.Data;
using HotelGestion.Infrastructure.Repositories;
using System.Configuration;
using System.Data;
using System.Windows;

namespace HotelGestion.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public DbConnectionFactory ConnectionFactory { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string connectionString =
                global::HotelGestion.UI.Properties.Settings.Default.HotelDB;

            ConnectionFactory =
            new DbConnectionFactory(connectionString);
            
        }
    }
}
