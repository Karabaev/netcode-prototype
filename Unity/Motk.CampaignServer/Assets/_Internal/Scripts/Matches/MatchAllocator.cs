using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Matchmaking;
using Motk.Shared.Core;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  [UsedImplicitly]
  public class MatchAllocator : ITickable
  {
    private const int ServerId = 0;
    
    private readonly MatchesState _matchesState;
    private readonly MatchmakingService _matchmakingService;
    private readonly AppScopeState _appScopeState;

    public MatchAllocator(MatchesState matchesState, MatchmakingService matchmakingService, AppScopeState appScopeState)
    {
      _matchesState = matchesState;
      _matchmakingService = matchmakingService;
      _appScopeState = appScopeState;
    }

    void ITickable.Tick() => PollRoomsAsync().Forget();

    private async UniTaskVoid PollRoomsAsync()
    {
      var roomIds = await _matchmakingService.GetRoomsForServerAsync(ServerId);

      var matchIds = _matchesState.Matches.Select(m => m.Key).ToList();

      var idsToCreate = roomIds.Except(matchIds);
      foreach (var id in idsToCreate)
      {
        var matchScope = _appScopeState.AppScope.CreateChild(MatchScopeInstaller.ConfigureScope);
        _matchesState.Matches.Add(id, new MatchState(matchScope));
      }
      
      var idsToRemove = matchIds.Except(roomIds);
      foreach (var id in idsToRemove)
      {
        _matchesState.Matches.Remove(id, out var matchState);
        matchState.Scope.Dispose();
      }
    }
  }
}