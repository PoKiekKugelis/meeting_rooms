using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Infrastructure.Repositories;
using MeetingRooms.Infrastructure.Queries;

namespace MeetingRooms.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services)
  {
    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("MeetingRooms"));
    services.AddScoped<IRoomRepository, RoomRepository>();
    services.AddScoped<IReservationRepository, ReservationRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserQueries, UserQueries>();
    services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
    return services;
  }
}
