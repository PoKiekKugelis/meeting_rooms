using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IUnitOfWork
{
  public DbSet<Room> Rooms => Set<Room>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Reservation> Reservations => Set<Reservation>();
  public DbSet<RoomEquipment> RoomEquipments => Set<RoomEquipment>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Room>(entity =>
    {
      entity.HasKey(r => r.Id);

      entity.Property(r => r.Name)
              .IsRequired()
              .HasMaxLength(100);

      entity.Property(r => r.Location)
              .IsRequired()
              .HasMaxLength(150);

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
              .HasMaxLength(100);

      entity.Property(u => u.LastName)
              .IsRequired()
              .HasMaxLength(100);

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
              .HasMaxLength(200);
    });

    modelBuilder.Entity<RoomEquipment>(entity =>
    {
      entity.HasKey(re => new { re.RoomId, re.EquipmentId });

      entity.HasOne(re => re.Room)
                .WithMany(r => r.Equipment)
                .HasForeignKey(re => re.RoomId);

      entity.HasOne(re => re.Equipment)
                .WithMany()
                .HasForeignKey(re => re.EquipmentId);
    });
  }
}
