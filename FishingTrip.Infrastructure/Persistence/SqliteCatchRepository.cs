using FishingTrip.Application.Abstractions;
using FishingTrip.Domain.Entities;

namespace FishingTrip.Infrastructure.Persistence;

public sealed class SqliteCatchRepository : ICatchRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteCatchRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IReadOnlyCollection<CatchRecord> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, AnglerId, Species, WeightInKg, LengthInCm, CaughtAt, Location, Note
            FROM Catches
            ORDER BY CaughtAt DESC;
            """;

        using var reader = command.ExecuteReader();
        var catches = new List<CatchRecord>();

        while (reader.Read())
        {
            catches.Add(new CatchRecord(
                Guid.Parse(reader.GetString(0)),
                Guid.Parse(reader.GetString(1)),
                reader.GetString(2),
                Convert.ToDecimal(reader.GetDouble(3)),
                Convert.ToDecimal(reader.GetDouble(4)),
                DateTime.Parse(reader.GetString(5), null, System.Globalization.DateTimeStyles.RoundtripKind),
                reader.GetString(6),
                reader.IsDBNull(7) ? null : reader.GetString(7)));
        }

        return catches;
    }

    public void Add(CatchRecord catchRecord)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

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
