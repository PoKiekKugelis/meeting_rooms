using Mapster;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Mapping;

/// <summary>
/// Provides configuration for mapping between domain entities and data transfer objects (DTOs) using Mapster
/// </summary>
public static class MapsterConfiguration
{
  public static TypeAdapterConfig Create()
  {
    var config = new TypeAdapterConfig();

    config.NewConfig<Room, RoomDto>()
        .Map(destination => destination.Status, source => source.Status.ToString())
        .Map(
            destination => destination.Equipment,
            source => source.Equipment
                .Select(link => new RoomEquipmentDto(link.Equipment.Name, link.Quantity))
                .ToList());

    config.NewConfig<Reservation, ReservationDto>()
        .Map(destination => destination.RoomName, source => source.Room.Name)
        .Map(destination => destination.UserName, source => source.User.FirstName + " " + source.User.LastName)
        .Map(destination => destination.Title, source => source.ReservationTitle)
        .Map(destination => destination.Status, source => source.Status.ToString());

    config.NewConfig<User, UserDto>();

    return config;
  }
}
