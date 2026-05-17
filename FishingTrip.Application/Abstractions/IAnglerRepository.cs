using FishingTrip.Domain.Entities;

namespace FishingTrip.Application.Abstractions;

public interface IAnglerRepository
{
    IReadOnlyCollection<Angler> GetAll();

    Angler? GetById(Guid id);

    void Add(Angler angler);

    bool DeleteById(Guid id);
}
