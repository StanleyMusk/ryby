using FishingTrip.Application.Abstractions;
using FishingTrip.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        using var dbContext = _connectionFactory.CreateDbContext();

        return dbContext.Catches
            .AsNoTracking()
            .OrderByDescending(catchRecord => catchRecord.CaughtAt)
            .ToArray();
    }

    public void Add(CatchRecord catchRecord)
    {
        using var dbContext = _connectionFactory.CreateDbContext();

        dbContext.Catches.Add(catchRecord);
        dbContext.SaveChanges();
    }

    public bool DeleteById(Guid id)
    {
        using var dbContext = _connectionFactory.CreateDbContext();

        var catchRecord = dbContext.Catches.SingleOrDefault(catchRecord => catchRecord.Id == id);
        if (catchRecord is null)
        {
            return false;
        }

        dbContext.Catches.Remove(catchRecord);
        dbContext.SaveChanges();
        return true;
    }

    public int DeleteByAnglerId(Guid anglerId)
    {
        using var dbContext = _connectionFactory.CreateDbContext();

        var catches = dbContext.Catches
            .Where(catchRecord => catchRecord.AnglerId == anglerId)
            .ToArray();

        dbContext.Catches.RemoveRange(catches);
        dbContext.SaveChanges();
        return catches.Length;
    }
}
