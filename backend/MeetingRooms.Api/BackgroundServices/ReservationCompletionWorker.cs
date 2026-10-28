using MeetingRooms.Core.Configuration;
using MeetingRooms.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Api.BackgroundServices;

/// <summary>
/// Background service that periodically checks for expired reservations and marks them as completed
/// </summary>
/// <param name="scopeFactory">The service scope factory</param>
/// <param name="clock">The time provider</param>
/// <param name="options">The reservation completion options</param>
/// <param name="logger">The logger</param>
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
      await ScanAsync(stoppingToken);

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

  /// <summary>
  /// Sweeps for expired reservations and marks them as completed
  /// </summary>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  private async Task ScanAsync(CancellationToken ct)
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
