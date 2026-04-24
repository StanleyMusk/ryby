namespace FishingTrip.Application.Contracts;

public sealed record RegisterCatchCommand(
    Guid AnglerId,
    string Species,
    decimal WeightInKg,
    decimal LengthInCm,
    DateTime CaughtAt,
    string Location,
    string? Note);
