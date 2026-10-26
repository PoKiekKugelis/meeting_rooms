using MapsterMapper;
using MeetingRooms.Core.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var frontendOrigins = builder.Configuration.GetSection("Cors:FrontendOrigins").Get<string[]>()
    ?? ["http://localhost:5173"];

builder.Services.AddCors(options => options.AddPolicy("AllowFrontend", policy => policy
    .WithOrigins(frontendOrigins).AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddSingleton(MapsterConfiguration.Create());
builder.Services.AddScoped<IMapper, ServiceMapper>();

var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
