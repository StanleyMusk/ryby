using FishingTrip.Application.Abstractions;
using FishingTrip.Application.Contracts;
using FishingTrip.Domain.Entities;

namespace FishingTrip.Application.Services;

public sealed class TripManagementService
{
    private readonly IAnglerRepository _anglerRepository;
    private readonly ICatchRepository _catchRepository;

    public TripManagementService(IAnglerRepository anglerRepository, ICatchRepository catchRepository)
    {
        _anglerRepository = anglerRepository;
        _catchRepository = catchRepository;
    }

    public TripDashboard GetDashboard()
    {
        var anglers = _anglerRepository
            .GetAll()
            .Select(angler => new AnglerSummary(angler.Id, FormatAnglerName(angler)))
            .OrderBy(angler => angler.DisplayName)
            .ToArray();

        var catches = _catchRepository
            .GetAll()
            .Select(record =>
            {
                var angler = _anglerRepository.GetById(record.AnglerId);
                var anglerName = angler is null
                    ? "Nieznany wędkarz"
                    : FormatAnglerName(angler);

                return new CatchSummary(
                    record.Id,
                    record.AnglerId,
                    anglerName,
                    record.Species,
                    record.WeightInKg,
                    record.LengthInCm,
                    record.CaughtAt);
            })
            .OrderByDescending(record => record.CaughtAt)
            .ToArray();

        return new TripDashboard("Weekendowy wyjazd wędkarski", anglers, catches);
    }

    public TripClassificationReport GetClassificationReport()
    {
        var anglers = _anglerRepository.GetAll();
        var catches = _catchRepository.GetAll();

        return new TripClassificationReport(
            "Klasyfikacja wyjazdu",
            anglers.Count,
            BuildClassificationSection("Klasyfikacja ogolna", anglers, catches, includeEmptyRows: true),
            BuildClassificationSection(
                "Drapiezniki",
                anglers,
                catches
                    .Where(record => FishSpeciesCatalog.GetKind(record.Species) == FishSpeciesCatalog.PredatorKind)
                    .ToArray(),
                includeEmptyRows: false),
            BuildClassificationSection(
                "Ryby niedrapiezne",
                anglers,
                catches
                    .Where(record => FishSpeciesCatalog.GetKind(record.Species) == FishSpeciesCatalog.OtherKind)
                    .ToArray(),
                includeEmptyRows: false));
    }

    public AnglerReport? GetAnglerReport(Guid anglerId)
    {
        var angler = _anglerRepository.GetById(anglerId);
        if (angler is null)
        {
            return null;
        }

        var catches = _catchRepository
            .GetAll()
            .Where(record => record.AnglerId == anglerId)
            .OrderByDescending(record => record.CaughtAt)
            .ToArray();

        var biggestCatch = catches
            .OrderByDescending(record => record.WeightInKg)
            .FirstOrDefault();

        var speciesBreakdown = catches
            .GroupBy(record => record.Species, StringComparer.OrdinalIgnoreCase)
            .Select(group => new SpeciesBreakdown(
                group.Key,
                group.Count(),
                group.Sum(record => record.WeightInKg),
                group.Max(record => record.WeightInKg)))
            .OrderByDescending(row => row.TotalWeightInKg)
            .ThenByDescending(row => row.CatchCount)
            .ThenBy(row => row.Species)
            .ToArray();

        var favoriteSpecies = speciesBreakdown.FirstOrDefault()?.Species ?? "-";

        var displayName = FormatAnglerName(angler);
        var summaries = catches
            .Select(record => new CatchSummary(
                record.Id,
                record.AnglerId,
                displayName,
                record.Species,
                record.WeightInKg,
                record.LengthInCm,
                record.CaughtAt))
            .ToArray();

        return new AnglerReport(
            angler.Id,
            displayName,
            catches.Length,
            catches.Sum(record => record.WeightInKg),
            biggestCatch?.WeightInKg ?? 0m,
            biggestCatch?.Species ?? "-",
            catches.Length == 0 ? 0m : catches.Average(record => record.WeightInKg),
            catches.Select(record => record.Species).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
            favoriteSpecies,
            catches.FirstOrDefault()?.CaughtAt,
            BuildFishKindBreakdown(catches),
            speciesBreakdown,
            summaries);
    }

    public CatchSummary RegisterCatch(RegisterCatchCommand command)
    {
        var angler = _anglerRepository.GetById(command.AnglerId)
            ?? throw new InvalidOperationException("Angler does not exist.");

        if (!FishSpeciesCatalog.Contains(command.Species))
        {
            throw new InvalidOperationException("Fish species is not supported.");
        }

        var catchRecord = new CatchRecord(
            Guid.NewGuid(),
            command.AnglerId,
            command.Species,
            command.WeightInKg,
            command.LengthInCm,
            command.CaughtAt,
            command.Note);

        _catchRepository.Add(catchRecord);

        return new CatchSummary(
            catchRecord.Id,
            catchRecord.AnglerId,
            FormatAnglerName(angler),
            catchRecord.Species,
            catchRecord.WeightInKg,
            catchRecord.LengthInCm,
            catchRecord.CaughtAt);
    }

    public AnglerSummary RegisterAngler(RegisterAnglerCommand command)
    {
        var angler = new Angler(
            Guid.NewGuid(),
            command.FirstName,
            command.LastName,
            command.Nickname);

        _anglerRepository.Add(angler);

        return new AnglerSummary(angler.Id, FormatAnglerName(angler));
    }

    public bool DeleteCatch(Guid catchId)
    {
        return _catchRepository.DeleteById(catchId);
    }

    public bool DeleteAngler(Guid anglerId)
    {
        if (_anglerRepository.GetById(anglerId) is null)
        {
            return false;
        }

        _catchRepository.DeleteByAnglerId(anglerId);
        return _anglerRepository.DeleteById(anglerId);
    }

    private static string FormatAnglerName(Angler angler)
    {
        var fullName = $"{angler.FirstName} {angler.LastName}";

        return string.IsNullOrWhiteSpace(angler.Nickname)
            ? fullName
            : $"{fullName} ({angler.Nickname})";
    }

    private static TripClassificationSection BuildClassificationSection(
        string title,
        IReadOnlyCollection<Angler> anglers,
        IReadOnlyCollection<CatchRecord> catches,
        bool includeEmptyRows)
    {
        var ranking = anglers
            .Select(angler =>
            {
                var anglerCatches = catches
                    .Where(record => record.AnglerId == angler.Id)
                    .ToArray();

                var biggestCatch = anglerCatches
                    .OrderByDescending(record => record.WeightInKg)
                    .FirstOrDefault();

                return new AnglerRankingRow(
                    angler.Id,
                    FormatAnglerName(angler),
                    anglerCatches.Length,
                    anglerCatches.Sum(record => record.WeightInKg),
                    biggestCatch?.WeightInKg ?? 0m,
                    biggestCatch?.Species ?? "-",
                    anglerCatches.Length == 0 ? 0m : anglerCatches.Average(record => record.WeightInKg),
                    anglerCatches.Select(record => record.Species).Distinct(StringComparer.OrdinalIgnoreCase).Count());
            })
            .Where(row => includeEmptyRows || row.CatchCount > 0)
            .OrderByDescending(row => row.TotalWeightInKg)
            .ThenByDescending(row => row.BiggestWeightInKg)
            .ThenByDescending(row => row.CatchCount)
            .ThenBy(row => row.DisplayName)
            .ToArray();

        var biggestCatch = catches
            .OrderByDescending(record => record.WeightInKg)
            .FirstOrDefault();

        return new TripClassificationSection(
            title,
            catches.Count,
            catches.Sum(record => record.WeightInKg),
            biggestCatch?.WeightInKg ?? 0m,
            biggestCatch?.Species ?? "-",
            catches.Count == 0 ? 0m : catches.Average(record => record.WeightInKg),
            catches.Select(record => record.Species).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
            ranking);
    }

    private static IReadOnlyCollection<FishKindBreakdown> BuildFishKindBreakdown(
        IReadOnlyCollection<CatchRecord> catches)
    {
        return catches
            .GroupBy(record => FishSpeciesCatalog.GetKind(record.Species), StringComparer.OrdinalIgnoreCase)
            .Select(group => new FishKindBreakdown(
                group.Key,
                group.Count(),
                group.Sum(record => record.WeightInKg),
                group.Max(record => record.WeightInKg),
                group.Select(record => record.Species).Distinct(StringComparer.OrdinalIgnoreCase).Count()))
            .OrderBy(row => row.Kind == FishSpeciesCatalog.PredatorKind ? 0 : 1)
            .ThenBy(row => row.Kind)
            .ToArray();
    }
}
