using Mapster;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Queries;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Mapping;

public static class MapsterConfiguration
{
  public static TypeAdapterConfig Create()
  {
    var config = new TypeAdapterConfig();
    config.NewConfig<Room, RoomDto>();
    config.NewConfig<Reservation, ReservationDto>()
        .Map(destination => destination.Title, source => source.ReservationTitle)
        .Map(destination => destination.RoomName, source => source.Room.Name)
        .Map(destination => destination.UserName, source => source.User.FirstName + " " + source.User.LastName)
        .Map(destination => destination.Status, source => source.Status.ToString());
    config.NewConfig<User, UserDto>();
    config.NewConfig<UserListItem, UserViewDto>();
    return config;
  }
}
