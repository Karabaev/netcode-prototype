using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.States;
using Motk.CampaignServer.Matchmaking;
using Motk.CampaignServer.Server.States;

namespace Motk.CampaignServer.Server
{
  [UsedImplicitly]
  public class RemoveMatchController : IDisposable
  {
    private readonly ServerState _serverState;
    private readonly MatchmakingClient _matchmakingClient;

    public RemoveMatchController(ServerState serverState, MatchmakingClient matchmakingClient)
    {
      _serverState = serverState;
      _matchmakingClient = matchmakingClient;
    }

    public void Initialize()
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
      newMatch.Users.ItemRemoved += State_OnMatchUserRemoved;
    }

    private void State_OnMatchRemoved(int matchId, MatchState removedMatch)
    {
      removedMatch.Users.ItemRemoved -= State_OnMatchUserRemoved;
    }
    
    private void State_OnMatchUserRemoved(string userSecret, ushort removedClientId) // todokmo оптимизировать
    {
      foreach (var (matchId, matchState) in _serverState.Matches.ToList())
      {
        if (matchState.Users.Count > 0) continue;

        _serverState.Matches.Remove(matchId);
        matchState.Scope.Dispose();
      }

      _matchmakingClient.RemoveUserFromRoomAsync(userSecret).Forget();
    }
  }
}