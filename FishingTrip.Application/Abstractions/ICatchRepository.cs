using FishingTrip.Domain.Entities;

namespace FishingTrip.Application.Abstractions;

public interface ICatchRepository
{
    IReadOnlyCollection<CatchRecord> GetAll();

    void Add(CatchRecord catchRecord);
}
