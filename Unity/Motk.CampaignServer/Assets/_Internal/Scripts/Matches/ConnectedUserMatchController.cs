using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Matches.States;
using Motk.Matchmaking;
using VContainer.Unity;

namespace Motk.CampaignServer.Matches
{
  [UsedImplicitly]
  public class ConnectedUserMatchController : IStartable, IDisposable
  {
    private readonly MatchesState _matchesState;
    private readonly MatchmakingService _matchmakingService;

    public ConnectedUserMatchController(MatchesState matchesState, MatchmakingService matchmakingService)
    {
      _matchesState = matchesState;
      _matchmakingService = matchmakingService;
    }

    void IStartable.Start()
    {
      _matchesState.Matches.ItemAdded += State_OnMatchAdded;
      _matchesState.Matches.ItemRemoved += State_OnMatchRemoved;
    }
    
    void IDisposable.Dispose()
    {
      foreach (var (matchId, matchState) in _matchesState.Matches)
        State_OnMatchRemoved(matchId, matchState);

      _matchesState.Matches.ItemAdded -= State_OnMatchAdded;
      _matchesState.Matches.ItemRemoved -= State_OnMatchRemoved;
    }

    private void State_OnMatchAdded(int matchId, MatchState newMatch)
    {
      newMatch.Users.ItemAdded += State_OnUserAdded;
      newMatch.Users.ItemRemoved += State_OnUserRemoved;
    }

    private void State_OnMatchRemoved(int matchId, MatchState removedMatch)
    {
      removedMatch.Users.ItemAdded -= State_OnUserAdded;
      removedMatch.Users.ItemRemoved -= State_OnUserRemoved;
    }
    
    private void State_OnUserAdded(string key, ulong newValue)
    {
    }

    private void State_OnUserRemoved(string userSecret, ulong removedClientId)
    {
      foreach (var (matchId, matchState) in _matchesState.Matches.ToList())
      {
        if (matchState.Users.Count > 0) continue;

        _matchesState.Matches.Remove(matchId);
        matchState.Scope.Dispose();
      }

      _matchmakingService.RemoveUserFromRoomAsync(userSecret).Forget();
    }
  }
}