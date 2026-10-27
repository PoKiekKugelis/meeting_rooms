using Microsoft.EntityFrameworkCore;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Infrastructure.Data;

public static class DatabaseSeeder
{
  public static async Task SeedAsync(AppDbContext db, TimeProvider clock, CancellationToken ct = default)
  {
    if (await db.Rooms.AnyAsync(ct))
    {
      return;
    }

    var now = clock.GetUtcNow().UtcDateTime;

    var vienas = new Room("Vienas", 12, "Floor 1");
    var du = new Room("Du", 8, "Floor 1");
    var trys = new Room("Trys", 20, "Floor 2");
    var keturi = new Room("Keturi", 4, "Floor 2");
    keturi.Deactivate();

    var petras = new User("Petras", "Petraitis");
    var lukas = new User("Lukas", "Lukaitis");
    var jonas = new User("Jonas", "Jonaitis");

    db.Rooms.AddRange(vienas, du, trys, keturi);
    db.Users.AddRange(petras, lukas, jonas);
    await db.SaveChangesAsync(ct);

    var cancelled = new Reservation(
        "Cancelled planning",
        At(now, 2, 10),
        At(now, 2, 11),
        3,
        du,
        lukas);
    cancelled.Cancel(now);

    // The hosted completion worker transitions this one on startup.
    var expired = new Reservation(
        "Expired retrospective",
        now.AddHours(-3),
        now.AddHours(-2),
        4,
        vienas,
        petras);

    // Lets the explicit completion endpoint be exercised while the meeting is running.
    var ongoing = new Reservation(
        "Ongoing review",
        now.AddHours(-1),
        now.AddHours(1),
        4,
        trys,
        jonas);

    db.Reservations.AddRange(
        new Reservation(
            "Sprint planning",
            At(now, 1, 10),
            At(now, 1, 11),
            5,
            vienas,
            petras),
        new Reservation(
            "Architecture review",
            At(now, 2, 14),
            At(now, 2, 16),
            6,
            trys,
            jonas),
        cancelled,
        expired,
        ongoing);

    await db.SaveChangesAsync(ct);
  }

  private static DateTime At(DateTime now, int days, int hour) =>
      DateTime.SpecifyKind(now.Date.AddDays(days).AddHours(hour), DateTimeKind.Utc);
}
