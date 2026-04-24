using FishingTrip.Application.Abstractions;
using FishingTrip.Domain.Entities;

namespace FishingTrip.Infrastructure.Persistence;

public sealed class SqliteAnglerRepository : IAnglerRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteAnglerRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IReadOnlyCollection<Angler> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FirstName, Nickname
            FROM Anglers
            ORDER BY FirstName, Nickname;
            """;

        using var reader = command.ExecuteReader();
        var anglers = new List<Angler>();

        while (reader.Read())
        {
            anglers.Add(new Angler(
                Guid.Parse(reader.GetString(0)),
                reader.GetString(1),
                reader.GetString(2)));
        }

        return anglers;
    }

    public Angler? GetById(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FirstName, Nickname
            FROM Anglers
            WHERE Id = @id
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("@id", id.ToString());

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Angler(
            Guid.Parse(reader.GetString(0)),
            reader.GetString(1),
            reader.GetString(2));
    }
}
