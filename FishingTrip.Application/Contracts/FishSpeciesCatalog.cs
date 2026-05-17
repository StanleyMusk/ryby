namespace FishingTrip.Application.Contracts;

public static class FishSpeciesCatalog
{
    public const string PredatorKind = "Drapiezniki";

    public const string OtherKind = "Pozostale ryby";

    public static IReadOnlyList<string> Species { get; } =
    [
        "Karp",
        "Szczupak",
        "Sandacz",
        "Sum",
        "Leszcz",
        "Ploc",
        "Okon",
        "Lin",
        "Karas",
        "Amur",
        "Wegorz",
        "Jaz",
        "Klen",
        "Brzana",
        "Bolen",
        "Pstrag potokowy",
        "Pstrag teczowy",
        "Lipien",
        "Mietus",
        "Troc wedrowna"
    ];

    private static readonly HashSet<string> PredatorSpecies = new(StringComparer.OrdinalIgnoreCase)
    {
        "Szczupak",
        "Sandacz",
        "Sum",
        "Okon",
        "Wegorz",
        "Bolen",
        "Pstrag potokowy",
        "Pstrag teczowy",
        "Mietus",
        "Troc wedrowna"
    };

    public static bool Contains(string species)
    {
        return Species.Contains(species, StringComparer.OrdinalIgnoreCase);
    }

    public static string GetKind(string species)
    {
        return PredatorSpecies.Contains(species)
            ? PredatorKind
            : OtherKind;
    }
}
