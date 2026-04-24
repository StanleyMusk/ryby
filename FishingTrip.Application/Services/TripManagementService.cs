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
            .Select(angler => new AnglerSummary(angler.Id, $"{angler.FirstName} ({angler.Nickname})"))
            .OrderBy(angler => angler.DisplayName)
            .ToArray();

        var catches = _catchRepository
            .GetAll()
            .Select(record =>
            {
                var angler = _anglerRepository.GetById(record.AnglerId);
                var anglerName = angler is null
                    ? "Nieznany wędkarz"
                    : $"{angler.FirstName} ({angler.Nickname})";

                return new CatchSummary(
                    record.Id,
                    anglerName,
                    record.Species,
                    record.WeightInKg,
                    record.LengthInCm,
                    record.CaughtAt,
                    record.Location);
            })
            .OrderByDescending(record => record.CaughtAt)
            .ToArray();

        return new TripDashboard("Weekendowy wyjazd wędkarski", anglers, catches);
    }

    public CatchSummary RegisterCatch(RegisterCatchCommand command)
    {
        var angler = _anglerRepository.GetById(command.AnglerId)
            ?? throw new InvalidOperationException("Angler does not exist.");

        var catchRecord = new CatchRecord(
            Guid.NewGuid(),
            command.AnglerId,
            command.Species,
            command.WeightInKg,
            command.LengthInCm,
            command.CaughtAt,
            command.Location,
            command.Note);

        _catchRepository.Add(catchRecord);

        return new CatchSummary(
            catchRecord.Id,
            $"{angler.FirstName} ({angler.Nickname})",
            catchRecord.Species,
            catchRecord.WeightInKg,
            catchRecord.LengthInCm,
            catchRecord.CaughtAt,
            catchRecord.Location);
    }
}
