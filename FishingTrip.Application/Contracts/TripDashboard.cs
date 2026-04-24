namespace FishingTrip.Application.Contracts;

public sealed record TripDashboard(
    string Title,
    IReadOnlyCollection<AnglerSummary> Anglers,
    IReadOnlyCollection<CatchSummary> Catches);
