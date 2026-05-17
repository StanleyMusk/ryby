using FishingTrip.Domain.Entities;

namespace FishingTrip.Application.Abstractions;

public interface ICatchRepository
{
    IReadOnlyCollection<CatchRecord> GetAll();

    void Add(CatchRecord catchRecord);

    bool DeleteById(Guid id);

    int DeleteByAnglerId(Guid anglerId);
}
