namespace FishingTrip.Application.Contracts;

public sealed record AnglerRankingRow(
    Guid AnglerId,
    string DisplayName,
    int CatchCount,
    decimal TotalWeightInKg,
    decimal BiggestWeightInKg,
    string BiggestSpecies,
    decimal AverageWeightInKg,
    int SpeciesCount);
