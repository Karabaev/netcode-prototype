using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Match;
using Motk.CampaignServer.Server.States;
using Motk.Matchmaking;
using VContainer.Unity;

namespace Motk.CampaignServer.Server
{
  [UsedImplicitly]
  public class RemoveMatchController : IStartable, IDisposable
  {
    private readonly ServerState _serverState;
    private readonly MatchmakingService _matchmakingService;

    public RemoveMatchController(ServerState serverState, MatchmakingService matchmakingService)
    {
      _serverState = serverState;
      _matchmakingService = matchmakingService;
    }

    void IStartable.Start()
    {
      _serverState.Matches.ItemAdded += State_OnMatchAdded;
      _serverState.Matches.ItemRemoved += State_OnMatchRemoved;
    }
    
    void IDisposable.Dispose()
    {
      foreach (var (matchId, matchState) in _serverState.Matches)
        State_OnMatchRemoved(matchId, matchState);

      _serverState.Matches.ItemAdded -= State_OnMatchAdded;
      _serverState.Matches.ItemRemoved -= State_OnMatchRemoved;
    }

    private void State_OnMatchAdded(int matchId, MatchState newMatch)
    {
      newMatch.Users.ItemRemoved += State_OnUserRemoved;
    }

    private void State_OnMatchRemoved(int matchId, MatchState removedMatch)
    {
      removedMatch.Users.ItemRemoved -= State_OnUserRemoved;
    }
    
    private void State_OnUserRemoved(string userSecret, ulong removedClientId)
    {
      foreach (var (matchId, matchState) in _serverState.Matches.ToList())
      {
        if (matchState.Users.Count > 0) continue;

        _serverState.Matches.Remove(matchId);
        matchState.Scope.Dispose();
      }

      _matchmakingService.RemoveUserFromRoomAsync(userSecret).Forget();
    }
  }
}