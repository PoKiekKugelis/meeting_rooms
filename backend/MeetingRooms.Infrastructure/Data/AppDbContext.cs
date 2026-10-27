using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
  private static readonly SemaphoreSlim WriteLock = new(1, 1);

  public DbSet<Room> Rooms => Set<Room>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Reservation> Reservations => Set<Reservation>();
  public DbSet<Equipment> Equipments => Set<Equipment>();
  public DbSet<RoomEquipment> RoomEquipments => Set<RoomEquipment>();

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
