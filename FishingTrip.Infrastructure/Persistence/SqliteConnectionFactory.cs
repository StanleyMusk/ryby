using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace FishingTrip.Infrastructure.Persistence;

public sealed class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string databasePath)
    {
        var directory = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();
    }

    public FishingTripDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FishingTripDbContext>()
            .UseSqlite(_connectionString)
            .Options;

        return new FishingTripDbContext(options);
    }
}
