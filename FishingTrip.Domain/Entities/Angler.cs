namespace FishingTrip.Domain.Entities;

public sealed class Angler
{
    public Angler(Guid id, string firstName, string nickname)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name is required.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(nickname))
        {
            throw new ArgumentException("Nickname is required.", nameof(nickname));
        }

        Id = id;
        FirstName = firstName.Trim();
        Nickname = nickname.Trim();
    }

    public Guid Id { get; }

    public string FirstName { get; }

    public string Nickname { get; }
}
