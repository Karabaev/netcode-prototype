using System;
using Motk.Matchmaking;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;

namespace Motk.CampaignServer.Matches
{
  public class PlayerToMatchConnector : MessageReceiver<AttachToMatchRequest>
  {
    private readonly NetworkManager _networkManager;
    private readonly MatchesState _matchesState;
    private readonly MatchmakingService _matchmakingService;

    public PlayerToMatchConnector(NetworkManager networkManager, MatchesState matchesState,
      MatchmakingService matchmakingService) : base(networkManager)
    {
      _networkManager = networkManager;
      _matchesState = matchesState;
      _matchmakingService = matchmakingService;
    }

    protected override void OnMessageReceived(ulong senderId, AttachToMatchRequest message)
    {
      
    }

    private async void OnAddToMatchRequested(ulong clientId, FastBufferReader reader)
    {
      reader.ReadValueSafe(out AttachToMatchRequest request);

      var matchState = _matchesState.Matches.Require(request.MatchId);

      var roomId = await _matchmakingService.GetRoomIdForUser(request.UserSecret);
      if (roomId != request.MatchId)
      {
        _networkManager.DisconnectClient(clientId);
        return;
      }

      _matchmakingService.FindRoom()
      
      // игроку не разрешено подключаться к комнате
      if (!matchState.UserIds.Contains(request.UserId))
      {
        throw new NotImplementedException("Return connection error");
      }

      
      // request.UserId
    }
  }
}