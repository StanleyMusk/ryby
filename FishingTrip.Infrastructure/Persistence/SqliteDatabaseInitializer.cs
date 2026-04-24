using FishingTrip.Domain.Entities;
using Microsoft.Data.Sqlite;

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
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        CreateTables(connection);
        SeedData(connection);
    }

    private static void CreateTables(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Anglers (
                Id TEXT PRIMARY KEY,
                FirstName TEXT NOT NULL,
                Nickname TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Catches (
                Id TEXT PRIMARY KEY,
                AnglerId TEXT NOT NULL,
                Species TEXT NOT NULL,
                WeightInKg REAL NOT NULL,
                LengthInCm REAL NOT NULL,
                CaughtAt TEXT NOT NULL,
                Location TEXT NOT NULL,
                Note TEXT NULL,
                FOREIGN KEY (AnglerId) REFERENCES Anglers(Id)
            );
            """;

        command.ExecuteNonQuery();
    }

    private static void SeedData(SqliteConnection connection)
    {
        if (HasAnyAnglers(connection))
        {
            return;
        }

        Angler[] anglers =
        [
            new Angler(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Kuba", "Sumiarz"),
            new Angler(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Marek", "Szczupak"),
            new Angler(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Olek", "Sandacz")
        ];

        foreach (var angler in anglers)
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                """
                INSERT INTO Anglers (Id, FirstName, Nickname)
                VALUES (@id, @firstName, @nickname);
                """;
            command.Parameters.AddWithValue("@id", angler.Id.ToString());
            command.Parameters.AddWithValue("@firstName", angler.FirstName);
            command.Parameters.AddWithValue("@nickname", angler.Nickname);
            command.ExecuteNonQuery();
        }

        CatchRecord[] catches =
        [
            new CatchRecord(
                Guid.NewGuid(),
                anglers[0].Id,
                "Sum",
                8.40m,
                102m,
                DateTime.Today.AddHours(-5),
                "Pomost polnocny",
                "Branie przed switem"),
            new CatchRecord(
                Guid.NewGuid(),
                anglers[1].Id,
                "Szczupak",
                4.15m,
                79m,
                DateTime.Today.AddHours(-2),
                "Zatoka trzcin",
                "Na gume")
        ];

        foreach (var catchRecord in catches)
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                """
                INSERT INTO Catches (Id, AnglerId, Species, WeightInKg, LengthInCm, CaughtAt, Location, Note)
                VALUES (@id, @anglerId, @species, @weightInKg, @lengthInCm, @caughtAt, @location, @note);
                """;
            command.Parameters.AddWithValue("@id", catchRecord.Id.ToString());
            command.Parameters.AddWithValue("@anglerId", catchRecord.AnglerId.ToString());
            command.Parameters.AddWithValue("@species", catchRecord.Species);
            command.Parameters.AddWithValue("@weightInKg", catchRecord.WeightInKg);
            command.Parameters.AddWithValue("@lengthInCm", catchRecord.LengthInCm);
            command.Parameters.AddWithValue("@caughtAt", catchRecord.CaughtAt.ToString("O"));
            command.Parameters.AddWithValue("@location", catchRecord.Location);
            command.Parameters.AddWithValue("@note", (object?)catchRecord.Note ?? DBNull.Value);
            command.ExecuteNonQuery();
        }
    }

    private static bool HasAnyAnglers(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT EXISTS(SELECT 1 FROM Anglers LIMIT 1);";

        var result = command.ExecuteScalar();
        return result is long value && value == 1;
    }
}
