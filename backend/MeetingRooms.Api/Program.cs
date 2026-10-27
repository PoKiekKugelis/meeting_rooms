using FluentValidation;
using MapsterMapper;
using MeetingRooms.Api.BackgroundServices;
using MeetingRooms.Core.Policies;
using MeetingRooms.Core.Configuration;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Mapping;
using MeetingRooms.Core.Services;
using MeetingRooms.Core.Validators;
using MeetingRooms.Infrastructure;
using MeetingRooms.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var frontendOrigins = builder.Configuration.GetSection("Cors:FrontendOrigins").Get<string[]>()
    ?? ["http://localhost:5173"];

builder.Services.AddCors(options => options.AddPolicy("AllowFrontend", policy => policy
    .WithOrigins(frontendOrigins).AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddSingleton(MapsterConfiguration.Create());
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddInfrastructure();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationBookingPolicy, ReservationBookingPolicy>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddValidatorsFromAssemblyContaining<ReservationRequestValidator>();
builder.Services.AddOptions<ReservationBookingOptions>()
    .BindConfiguration(ReservationBookingOptions.SectionName)
    .Validate(
        options => options.MaximumDurationDays <= options.MaximumBookingHorizonDays,
        "ReservationBooking:MaximumDurationDays must not exceed MaximumBookingHorizonDays.")
    .ValidateOnStart();
builder.Services.AddOptions<ReservationCompletionOptions>()
    .BindConfiguration(ReservationCompletionOptions.SectionName)
    .ValidateOnStart();
builder.Services.AddHostedService<ReservationCompletionWorker>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("AllowFrontend");
if (!app.Environment.IsDevelopment())
{
  app.UseHttpsRedirection();
}
app.MapControllers();
app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  var clock = scope.ServiceProvider.GetRequiredService<TimeProvider>();
  await DatabaseSeeder.SeedAsync(db, clock);
}

app.Run();
