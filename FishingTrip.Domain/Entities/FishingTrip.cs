namespace FishingTrip.Domain.Entities;

public sealed class FishingTrip
{
    public FishingTrip(Guid id, string name, DateOnly startDate, DateOnly endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Trip name is required.", nameof(name));
        }

        if (endDate < startDate)
        {
            throw new ArgumentException("End date cannot be earlier than start date.", nameof(endDate));
        }

        Id = id;
        Name = name.Trim();
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid Id { get; }

    public string Name { get; }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }
}
