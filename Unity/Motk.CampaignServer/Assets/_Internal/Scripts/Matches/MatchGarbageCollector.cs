using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
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

    void ITickable.Tick()
    {
      foreach (var (matchId, matchScope) in _matchesState.Matches)
      {
        if (HasConnectedPlayers(matchScope)) 
          continue;
        
        DeleteMatch(matchId);
      }
    }

    private bool HasConnectedPlayers(MatchState matchState)
    {
      return matchState.UserIds.Count > 0;
    }

    private void DeleteMatch(int matchId) => _matchmakingService.DeleteRoomAsync(matchId).Forget();
  }
}