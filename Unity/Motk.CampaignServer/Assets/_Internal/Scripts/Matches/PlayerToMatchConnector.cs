using System;
using Motk.Shared.Matches;
using Unity.Netcode;

namespace Motk.CampaignServer.Matches
{
  public class PlayerToMatchConnector : IDisposable
  {
    private readonly NetworkManager _networkManager;
    private readonly MatchesState _matchesState;

    public PlayerToMatchConnector(NetworkManager networkManager, MatchesState matchesState)
    {
      _networkManager = networkManager;
      _matchesState = matchesState;
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(nameof(AddToMatchRequest), OnAddToMatchRequested);
    }

    public void Dispose()
    {
      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(nameof(AddToMatchRequest), OnAddToMatchRequested());
    }

    private void OnAddToMatchRequested(ulong clientId, FastBufferReader reader)
    {
      reader.ReadValueSafe(out AddToMatchRequest request);

      var matchState = _matchesState.Matches.Require(request.MatchId);

      // игроку не разрешено подключаться к комнате
      if (!matchState.UserIds.Contains(request.UserId))
      {
        throw new NotImplementedException("Return connection error");
      }

      
      request.UserId
    }
  }
}