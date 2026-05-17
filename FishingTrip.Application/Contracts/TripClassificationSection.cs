namespace FishingTrip.Application.Contracts;

public sealed record TripClassificationSection(
    string Title,
    int CatchCount,
    decimal TotalWeightInKg,
    decimal BiggestWeightInKg,
    string BiggestSpecies,
    decimal AverageWeightInKg,
    int SpeciesCount,
    IReadOnlyCollection<AnglerRankingRow> Ranking);
