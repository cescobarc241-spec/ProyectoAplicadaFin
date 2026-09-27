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

            string server = Environment.MachineName;

            string connectionString =
                $"Data Source={server};Initial Catalog=HotelDB;Integrated Security=True;TrustServerCertificate=True";

            ConnectionFactory =
                new DbConnectionFactory(connectionString);
        }
    }
}
