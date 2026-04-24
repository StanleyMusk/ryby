namespace FishingTrip.Domain.Entities;

public sealed class CatchRecord
{
    public CatchRecord(
        Guid id,
        Guid anglerId,
        string species,
        decimal weightInKg,
        decimal lengthInCm,
        DateTime caughtAt,
        string location,
        string? note)
    {
        if (anglerId == Guid.Empty)
        {
            throw new ArgumentException("Angler id is required.", nameof(anglerId));
        }

        if (string.IsNullOrWhiteSpace(species))
        {
            throw new ArgumentException("Species is required.", nameof(species));
        }

        if (weightInKg <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weightInKg), "Weight must be greater than zero.");
        }

        if (lengthInCm <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lengthInCm), "Length must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location is required.", nameof(location));
        }

        Id = id;
        AnglerId = anglerId;
        Species = species.Trim();
        WeightInKg = weightInKg;
        LengthInCm = lengthInCm;
        CaughtAt = caughtAt;
        Location = location.Trim();
        Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
    }

    public Guid Id { get; }

    public Guid AnglerId { get; }

    public string Species { get; }

    public decimal WeightInKg { get; }

    public decimal LengthInCm { get; }

    public DateTime CaughtAt { get; }

    public string Location { get; }

    public string? Note { get; }
}
