namespace FishingTrip.Application.Contracts;

public sealed record CatchSummary(
    Guid Id,
    Guid AnglerId,
    string AnglerName,
    string Species,
    decimal WeightInKg,
    decimal LengthInCm,
    DateTime CaughtAt);
