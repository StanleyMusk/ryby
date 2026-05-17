namespace FishingTrip.Application.Contracts;

public sealed record FishKindBreakdown(
    string Kind,
    int CatchCount,
    decimal TotalWeightInKg,
    decimal BiggestWeightInKg,
    int SpeciesCount);
