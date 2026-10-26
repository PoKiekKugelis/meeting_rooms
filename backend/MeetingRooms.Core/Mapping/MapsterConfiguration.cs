using Mapster;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Mapping;

public static class MapsterConfiguration
{
  public static TypeAdapterConfig Create()
  {
    var config = new TypeAdapterConfig();
    config.NewConfig<Room, RoomDto>();
    config.NewConfig<Reservation, ReservationDto>();
    config.NewConfig<User, UserDto>();
    return config;
  }
}
