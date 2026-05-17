namespace FishingTrip.Application.Contracts;

public sealed record TripClassificationReport(
    string Title,
    int AnglerCount,
    TripClassificationSection Overall,
    TripClassificationSection Predators,
    TripClassificationSection OtherFish);
