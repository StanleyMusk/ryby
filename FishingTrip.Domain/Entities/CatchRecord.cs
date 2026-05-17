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

        Id = id;
        AnglerId = anglerId;
        Species = species.Trim();
        WeightInKg = weightInKg;
        LengthInCm = lengthInCm;
        CaughtAt = caughtAt;
        Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
    }

    public Guid Id { get; }

    public Guid AnglerId { get; }

    public string Species { get; }

    public decimal WeightInKg { get; }

    public decimal LengthInCm { get; }

    public DateTime CaughtAt { get; }

    public string? Note { get; }
}
