using JetBrains.Annotations;
using Motk.CampaignServer.Matches.States;
using Motk.Matchmaking;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;
using VContainer;

namespace Motk.CampaignServer.Matches
{
  [UsedImplicitly]
  public class PlayerToMatchConnector : MessageReceiver<AttachToMatchRequest>
  {
    private readonly NetworkManager _networkManager;
    private readonly MatchesState _matchesState;
    private readonly MatchmakingService _matchmakingService;
    private readonly AppScopeState _appScopeState;
    private readonly ServerMessageSender _messageSender;

    public PlayerToMatchConnector(NetworkManager networkManager, MatchesState matchesState,
      MatchmakingService matchmakingService, AppScopeState appScopeState,
      ServerMessageSender messageSender) : base(networkManager)
    {
      _networkManager = networkManager;
      _matchesState = matchesState;
      _matchmakingService = matchmakingService;
      _appScopeState = appScopeState;
      _messageSender = messageSender;
    }

    protected override async void OnMessageReceived(ulong senderId, AttachToMatchRequest message)
    {
      // todokmo проверять еще serverId
      var roomId = await _matchmakingService.GetRoomIdForUser(message.UserSecret);
      
      // игроку не разрешено подключаться к комнате
      if (roomId != message.MatchId)
      {
        _networkManager.DisconnectClient(senderId);
        return;
      }
      
      if (!_matchesState.Matches.TryGet(roomId, out var match))
      {
        var matchScope = _appScopeState.AppScope.CreateChild(MatchScopeInstaller.ConfigureScope);
        match = matchScope.Container.Resolve<MatchState>();
        match.LocationId = await _matchmakingService.GetLocationIdForRoom(roomId);
        match.Scope = matchScope;
        _matchesState.Matches.Add(roomId, match);
      }
      
      match.Users.Add(message.UserSecret, senderId);
      _messageSender.Send(new AttachedToMatchCommand(), senderId);
    }
  }
}