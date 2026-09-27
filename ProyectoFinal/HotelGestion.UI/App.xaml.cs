using System.Windows;
using HotelGestion.Application.Interfaces;
using HotelGestion.Application.UseCases;
using HotelGestion.Infrastructure.Data;
using HotelGestion.Infrastructure.Repositories;

namespace HotelGestion.UI;

public partial class App : System.Windows.Application
{
    public DbConnectionFactory ConnectionFactory { get; private set; } = null!;

    public IHabitacionService HabitacionService { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        string server = Environment.MachineName;

        string connectionString =
            $"Data Source={server};Initial Catalog=HotelDB;Integrated Security=True;TrustServerCertificate=True";

        ConnectionFactory =
            new DbConnectionFactory(connectionString);

        // Repository
        var habitacionRepository =
            new HabitacionRepository(ConnectionFactory);

        // Service
        HabitacionService =
            new HabitacionService(habitacionRepository);
    }
}
