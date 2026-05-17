using FishingTrip.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FishingTrip.Infrastructure.Persistence;

public sealed class FishingTripDbContext : DbContext
{
    public FishingTripDbContext(DbContextOptions<FishingTripDbContext> options)
        : base(options)
    {
    }

    public DbSet<Angler> Anglers => Set<Angler>();

    public DbSet<CatchRecord> Catches => Set<CatchRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Angler>(entity =>
        {
            entity.ToTable("Anglers");
            entity.HasKey(angler => angler.Id);

            entity.Property(angler => angler.Id)
                .ValueGeneratedNever();

            entity.Property(angler => angler.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(angler => angler.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(angler => angler.Nickname)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<CatchRecord>(entity =>
        {
            entity.ToTable("Catches");
            entity.HasKey(catchRecord => catchRecord.Id);

            entity.Property(catchRecord => catchRecord.Id)
                .ValueGeneratedNever();

            entity.Property(catchRecord => catchRecord.AnglerId)
                .IsRequired();

            entity.Property(catchRecord => catchRecord.Species)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(catchRecord => catchRecord.WeightInKg)
                .HasConversion<double>()
                .IsRequired();

            entity.Property(catchRecord => catchRecord.LengthInCm)
                .HasConversion<double>()
                .IsRequired();

            entity.Property(catchRecord => catchRecord.CaughtAt)
                .HasConversion(
                    caughtAt => caughtAt.ToString("O"),
                    storedValue => DateTime.Parse(
                        storedValue,
                        null,
                        System.Globalization.DateTimeStyles.RoundtripKind))
                .IsRequired();

            entity.Property(catchRecord => catchRecord.Note)
                .HasMaxLength(500);

            entity.HasOne<Angler>()
                .WithMany()
                .HasForeignKey(catchRecord => catchRecord.AnglerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
