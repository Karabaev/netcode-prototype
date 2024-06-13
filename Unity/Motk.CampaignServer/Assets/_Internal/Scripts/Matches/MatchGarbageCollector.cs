using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Matches.States;
using Motk.Matchmaking;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  [UsedImplicitly]
  public class MatchGarbageCollector : ITickable
  {
    private readonly MatchesState _matchesState;
    private readonly MatchmakingService _matchmakingService;

    public MatchGarbageCollector(MatchesState matchesState, MatchmakingService matchmakingService)
    {
      _matchesState = matchesState;
      _matchmakingService = matchmakingService;
    }

    // todokmo вместо поллинга можно подписываться на события изменения юзеров в матчах
    void ITickable.Tick()
    {
      foreach (var (matchId, matchScope) in _matchesState.Matches)
      {
        if (HasConnectedPlayers(matchScope)) 
          continue;
        
        DeleteMatch(matchId);
      }
    }

    private bool HasConnectedPlayers(MatchState matchState) => matchState.Users.Count > 0;

    private void DeleteMatch(int matchId)
    {
      _matchesState.Matches.Remove(matchId, out var matchState);
      matchState.Scope.Dispose();
      _matchmakingService.DeleteRoomAsync(matchId).Forget();
    }
  }
}