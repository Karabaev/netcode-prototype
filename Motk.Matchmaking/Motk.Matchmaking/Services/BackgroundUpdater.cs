namespace Motk.Matchmaking.Services;

public class BackgroundUpdater : IHostedService
{
  private readonly MatchmakingService _matchmakingService;

  public BackgroundUpdater(MatchmakingService matchmakingService)
  {
    _matchmakingService = matchmakingService;
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
      _matchmakingService.Update();
    }
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}