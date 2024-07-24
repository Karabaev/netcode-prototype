namespace Motk.Matchmaking.Services;

public class BackgroundUpdater : IHostedService
{
  private readonly MatchmakingService _matchmakingService;
  private readonly ILogger<BackgroundUpdater> _logger;

  public BackgroundUpdater(MatchmakingService matchmakingService, ILogger<BackgroundUpdater> logger)
  {
    _matchmakingService = matchmakingService;
    _logger = logger;
  }
  public Task StartAsync(CancellationToken cancellationToken)
  {
    RunAsync(cancellationToken);
    return Task.CompletedTask;
  }

  private async Task RunAsync(CancellationToken cancellationToken)
  {
    while (!cancellationToken.IsCancellationRequested)
    {
      await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);

      try
      {
        _matchmakingService.Update();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occured while updating matchmaking");
      }
    }
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}