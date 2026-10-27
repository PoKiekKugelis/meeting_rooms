using MeetingRooms.Core.Configuration;
using MeetingRooms.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Api.BackgroundServices;

public sealed class ReservationCompletionWorker(
    IServiceScopeFactory scopeFactory,
    TimeProvider clock,
    IOptions<ReservationCompletionOptions> options,
    ILogger<ReservationCompletionWorker> logger) : BackgroundService
{
  private readonly TimeSpan _interval = TimeSpan.FromSeconds(options.Value.SweepIntervalSeconds);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    using var timer = new PeriodicTimer(_interval);

    while (!stoppingToken.IsCancellationRequested)
    {
      await SweepAsync(stoppingToken);

      try
      {
        if (!await timer.WaitForNextTickAsync(stoppingToken))
        {
          break;
        }
      }
      catch (OperationCanceledException)
      {
        break;
      }
    }
  }

  private async Task SweepAsync(CancellationToken ct)
  {
    try
    {
      using var scope = scopeFactory.CreateScope();
      var reservations = scope.ServiceProvider.GetRequiredService<IReservationService>();
      var completed = await reservations.CompleteExpiredAsync(clock.GetUtcNow().UtcDateTime, ct);

      if (completed > 0)
      {
        logger.LogInformation("Completed {Count} expired reservation(s).", completed);
      }
    }
    catch (OperationCanceledException) when (ct.IsCancellationRequested)
    {
    }
    catch (Exception exception)
    {
      logger.LogError(exception, "Failed to complete expired reservations.");
    }
  }
}
