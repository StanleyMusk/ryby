using Microsoft.EntityFrameworkCore;

namespace FishingTrip.Infrastructure.Persistence;

public sealed class SqliteDatabaseInitializer
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteDatabaseInitializer(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Initialize()
    {
        using var dbContext = _connectionFactory.CreateDbContext();

        dbContext.Database.EnsureCreated();
        UpdateSchema(dbContext);
    }

    private static void UpdateSchema(FishingTripDbContext dbContext)
    {
        var anglerColumns = dbContext.Database
            .SqlQueryRaw<string>("SELECT name AS Value FROM pragma_table_info('Anglers')")
            .ToArray();

        if (!anglerColumns.Contains("LastName", StringComparer.OrdinalIgnoreCase))
        {
            dbContext.Database.ExecuteSqlRaw(
                "ALTER TABLE Anglers ADD COLUMN LastName TEXT NOT NULL DEFAULT 'Nieznane'");
        }

        var catchColumns = dbContext.Database
            .SqlQueryRaw<string>("SELECT name AS Value FROM pragma_table_info('Catches')")
            .ToArray();

        if (catchColumns.Contains("Location", StringComparer.OrdinalIgnoreCase))
        {
            RebuildCatchesWithoutLocation(dbContext);
        }
    }

    private static void RebuildCatchesWithoutLocation(FishingTripDbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw(
            """
            CREATE TABLE IF NOT EXISTS Catches_new (
                Id TEXT NOT NULL CONSTRAINT PK_Catches PRIMARY KEY,
                AnglerId TEXT NOT NULL,
                Species TEXT NOT NULL,
                WeightInKg REAL NOT NULL,
                LengthInCm REAL NOT NULL,
                CaughtAt TEXT NOT NULL,
                Note TEXT NULL,
                CONSTRAINT FK_Catches_Anglers_AnglerId
                    FOREIGN KEY (AnglerId)
                    REFERENCES Anglers (Id)
                    ON DELETE RESTRICT
            );
            """);

        dbContext.Database.ExecuteSqlRaw(
            """
            INSERT INTO Catches_new (Id, AnglerId, Species, WeightInKg, LengthInCm, CaughtAt, Note)
            SELECT Id, AnglerId, Species, WeightInKg, LengthInCm, CaughtAt, Note
            FROM Catches;
            """);

        dbContext.Database.ExecuteSqlRaw("DROP TABLE Catches;");
        dbContext.Database.ExecuteSqlRaw("ALTER TABLE Catches_new RENAME TO Catches;");
        dbContext.Database.ExecuteSqlRaw("CREATE INDEX IF NOT EXISTS IX_Catches_AnglerId ON Catches (AnglerId);");
    }
}
