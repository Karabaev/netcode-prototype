using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Locations;
using Motk.CampaignServer.Matches.States;
using Motk.Matchmaking;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Motk.CampaignServer.Matches
{
  [UsedImplicitly]
  public class PlayerToMatchConnector : IDisposable
  {
    private readonly NetworkManager _networkManager;
    private readonly MatchesState _matchesState;
    private readonly MatchmakingService _matchmakingService;
    private readonly AppScopeState _appScopeState;
    private readonly ServerMessageSender _messageSender;
    private readonly ServerMessageReceiver _messageReceiver;

    public PlayerToMatchConnector(NetworkManager networkManager, MatchesState matchesState,
      MatchmakingService matchmakingService, AppScopeState appScopeState,
      ServerMessageSender messageSender, ServerMessageReceiver messageReceiver)
    {
      _networkManager = networkManager;
      _matchesState = matchesState;
      _matchmakingService = matchmakingService;
      _appScopeState = appScopeState;
      _messageSender = messageSender;
      _messageReceiver = messageReceiver;
      
      _messageReceiver.RegisterMessageHandler<AttachToMatchRequest>(Network_OnAttackToMatchRequested);
    }

    public void Dispose()
    {
      _messageReceiver.UnregisterMessageHandler<AttachToMatchRequest>();
    }

    private async void Network_OnAttackToMatchRequested(ulong senderId, AttachToMatchRequest message)
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
        match.Scope.Container.Resolve<LocationOffsetState>().Offset = Vector3.zero;
        _matchesState.Matches.Add(roomId, match);
      }

      await UniTask.Yield();
      
      _messageSender.Send(new AttachedToMatchCommand(), senderId);
      match.Users.Add(message.UserSecret, senderId);
    }
  }
}