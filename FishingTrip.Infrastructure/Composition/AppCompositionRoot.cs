using FishingTrip.Application.Services;
using FishingTrip.Infrastructure.Persistence;

namespace FishingTrip.Infrastructure.Composition;

public static class AppCompositionRoot
{
    public static TripManagementService CreateTripManagementService()
    {
        var databasePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FishingTripApp",
            "fishing-trip.db");

        var connectionFactory = new SqliteConnectionFactory(databasePath);
        var initializer = new SqliteDatabaseInitializer(connectionFactory);
        initializer.Initialize();

        var anglerRepository = new SqliteAnglerRepository(connectionFactory);
        var catchRepository = new SqliteCatchRepository(connectionFactory);

        return new TripManagementService(anglerRepository, catchRepository);
    }
}
