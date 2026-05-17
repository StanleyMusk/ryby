namespace FishingTrip.Domain.Entities;

public sealed class Angler
{
    public Angler(Guid id, string firstName, string lastName, string? nickname)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name is required.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name is required.", nameof(lastName));
        }

        Id = id;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Nickname = string.IsNullOrWhiteSpace(nickname) ? string.Empty : nickname.Trim();
    }

    public Guid Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Nickname { get; }
}
