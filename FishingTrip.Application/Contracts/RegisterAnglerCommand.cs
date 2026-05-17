namespace FishingTrip.Application.Contracts;

public sealed record RegisterAnglerCommand(string FirstName, string LastName, string? Nickname);
