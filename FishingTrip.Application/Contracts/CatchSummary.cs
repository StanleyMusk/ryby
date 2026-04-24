namespace FishingTrip.Application.Contracts;

public sealed record CatchSummary(
    Guid Id,
    string AnglerName,
    string Species,
    decimal WeightInKg,
    decimal LengthInCm,
    DateTime CaughtAt,
    string Location);
