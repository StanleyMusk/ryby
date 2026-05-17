using FishingTrip.Application.Abstractions;
using FishingTrip.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        using var dbContext = _connectionFactory.CreateDbContext();

        return dbContext.Anglers
            .AsNoTracking()
            .OrderBy(angler => angler.FirstName)
            .ThenBy(angler => angler.LastName)
            .ThenBy(angler => angler.Nickname)
            .ToArray();
    }

    public Angler? GetById(Guid id)
    {
        using var dbContext = _connectionFactory.CreateDbContext();

        return dbContext.Anglers
            .AsNoTracking()
            .SingleOrDefault(angler => angler.Id == id);
    }

    public void Add(Angler angler)
    {
        using var dbContext = _connectionFactory.CreateDbContext();

        dbContext.Anglers.Add(angler);
        dbContext.SaveChanges();
    }

    public bool DeleteById(Guid id)
    {
        using var dbContext = _connectionFactory.CreateDbContext();

        var angler = dbContext.Anglers.SingleOrDefault(angler => angler.Id == id);
        if (angler is null)
        {
            return false;
        }

        dbContext.Anglers.Remove(angler);
        dbContext.SaveChanges();
        return true;
    }
}
