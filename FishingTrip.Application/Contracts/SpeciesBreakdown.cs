namespace FishingTrip.Application.Contracts;

public sealed record SpeciesBreakdown(
    string Species,
    int CatchCount,
    decimal TotalWeightInKg,
    decimal BiggestWeightInKg);
