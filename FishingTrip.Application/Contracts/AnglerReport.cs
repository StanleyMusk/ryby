namespace FishingTrip.Application.Contracts;

public sealed record AnglerReport(
    Guid AnglerId,
    string DisplayName,
    int CatchCount,
    decimal TotalWeightInKg,
    decimal BiggestWeightInKg,
    string BiggestSpecies,
    decimal AverageWeightInKg,
    int SpeciesCount,
    string FavoriteSpecies,
    DateTime? LastCatchAt,
    IReadOnlyCollection<FishKindBreakdown> FishKindBreakdown,
    IReadOnlyCollection<SpeciesBreakdown> SpeciesBreakdown,
    IReadOnlyCollection<CatchSummary> Catches);
