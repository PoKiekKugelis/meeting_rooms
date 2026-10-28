using Microsoft.EntityFrameworkCore;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Infrastructure.Data;

/// <summary>
/// Provides methods to seed the database with initial data for rooms, users, equipment, and reservations
/// </summary>
public static class DatabaseSeeder
{

    /// <summary>
    /// Seeds the database with initial data for rooms, users, equipment, and reservations if the database is empty
    /// </summary>
    /// <param name="db">The application's database context</param>
    /// <param name="clock">The time provider</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
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

    var projector = new Equipment("Projector");
    var tvScreen = new Equipment("TV screen");
    var whiteboard = new Equipment("Whiteboard");

    db.RoomEquipments.AddRange(
        new RoomEquipment(vienas, projector, quantity: 2),
        new RoomEquipment(vienas, whiteboard),
        new RoomEquipment(du, tvScreen),
        new RoomEquipment(du, whiteboard),
        new RoomEquipment(trys, projector),
        new RoomEquipment(trys, tvScreen, quantity: 2),
        new RoomEquipment(keturi, whiteboard));

    var petras = new User("Petras", "Petraitis");
    var lukas = new User("Lukas", "Lukaitis");
    var jonas = new User("Jonas", "Jonaitis");

    db.Rooms.AddRange(vienas, du, trys, keturi);
    db.Users.AddRange(petras, lukas, jonas);
    db.Equipments.AddRange(projector, tvScreen, whiteboard);
    await db.SaveChangesAsync(ct);

    var cancelled = new Reservation(
        "Cancelled planning",
        At(now, 2, 10),
        At(now, 2, 11),
        3,
        du,
        lukas);
    cancelled.Cancel(now);

    // The completion worker changes the status of this one to "Completed" on startup.
    var expired = new Reservation(
        "Old reservation",
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

  /// <summary>
  /// Calculates a DateTime that is a specified number of days and hours from a given DateTime, normalized to UTC
  /// </summary>
  /// <param name="now">The starting DateTime</param>
  /// <param name="days">The number of days to add</param>
  /// <param name="hour">The hour to set</param>
  /// <returns>The calculated DateTime</returns>
  private static DateTime At(DateTime now, int days, int hour) =>
      DateTime.SpecifyKind(now.Date.AddDays(days).AddHours(hour), DateTimeKind.Utc);
}
