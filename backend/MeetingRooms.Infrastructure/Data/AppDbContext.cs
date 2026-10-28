using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Infrastructure.Data;

/// <summary>
/// Represents the application's database context, providing access to the database and managing entity sets
/// </summary>
/// <param name="options">The options for configuring the database context</param>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
  /// <summary>
  /// A semaphore to ensure that write operations are serialized, preventing concurrent reservation writes to the database
  /// </summary>
  private static readonly SemaphoreSlim WriteLock = new(1, 1);

  /// <summary>
  /// The DbSet for rooms
  /// </summary>
  public DbSet<Room> Rooms => Set<Room>();
  /// <summary>
  /// The DbSet for reservations
  /// </summary>
  public DbSet<User> Users => Set<User>();
  /// <summary>
  /// The DbSet for equipment
  /// </summary>
  public DbSet<Reservation> Reservations => Set<Reservation>();
  /// <summary>
  /// The DbSet for equipment
  /// </summary>
  public DbSet<Equipment> Equipments => Set<Equipment>();
  /// <summary>
  /// The DbSet for room equipment associations
  /// </summary>
  public DbSet<RoomEquipment> RoomEquipments => Set<RoomEquipment>();

  /// <summary>
  /// Ensures that write operations are serialized, preventing concurrent reservation writes to the database
  /// </summary>
  /// <typeparam name="T">The type of the result value</typeparam>
  /// <param name="operation">The operation to serialize</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The result of the operation</returns>
  public async Task<T> SerializeWrites<T>(Func<CancellationToken, Task<T>> operation, CancellationToken ct)
  {
    await WriteLock.WaitAsync(ct);
    try
    {
      return await operation(ct);
    }
    finally
    {
      WriteLock.Release();
    }
  }

  /// <summary>
  /// Configures the model for the database context, defining entity relationships and constraints
  /// </summary>
  /// <param name="modelBuilder">The model builder</param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Room>(entity =>
    {
      entity.HasKey(r => r.Id);

      entity.Property(r => r.Name)
              .IsRequired()
              .HasMaxLength(Room.MaxNameLength);

      entity.Property(r => r.Location)
              .IsRequired()
              .HasMaxLength(Room.MaxLocationLength);

      entity.HasMany(r => r.Reservations)
              .WithOne(res => res.Room)
              .HasForeignKey(res => res.RoomId)
              .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(u => u.Id);

      entity.Property(u => u.FirstName)
              .IsRequired()
              .HasMaxLength(User.MaxNameLength);

      entity.Property(u => u.LastName)
              .IsRequired()
              .HasMaxLength(User.MaxNameLength);

      entity.HasMany(u => u.Reservations)
              .WithOne(res => res.User)
              .HasForeignKey(res => res.UserId)
              .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<Reservation>(entity =>
    {
      entity.HasKey(r => r.Id);

      entity.Property(r => r.ReservationTitle)
              .IsRequired()
              .HasMaxLength(Reservation.MaxTitleLength);
    });

    modelBuilder.Entity<Equipment>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Name)
          .IsRequired()
          .HasMaxLength(Equipment.MaxNameLength);
    });

    modelBuilder.Entity<RoomEquipment>(entity =>
    {
      entity.HasKey(re => new { re.RoomId, re.EquipmentId });

      entity.HasOne(re => re.Room)
                .WithMany(r => r.Equipment)
                .HasForeignKey(re => re.RoomId);

      entity.HasOne(re => re.Equipment)
                .WithMany(equipment => equipment.Rooms)
                .HasForeignKey(re => re.EquipmentId);
    });
  }
}
