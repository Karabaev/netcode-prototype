namespace Motk.Matchmaking.New;

public class Updater : IHostedService
{
  private readonly MatchmakingService _matchmakingService;

  public Updater(MatchmakingService matchmakingService)
  {
    _matchmakingService = matchmakingService;
  }
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Run(cancellationToken);
    return Task.CompletedTask;
  }

  private async Task Run(CancellationToken cancellationToken)
  {
    while (!cancellationToken.IsCancellationRequested)
    {
      await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
      _matchmakingService.Update();
    }
  }

  public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}