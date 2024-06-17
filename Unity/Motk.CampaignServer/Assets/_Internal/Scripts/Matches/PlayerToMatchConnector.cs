using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Matches.States;
using Motk.Matchmaking;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;
using UnityEngine;

namespace Motk.CampaignServer.Matches
{
  [UsedImplicitly]
  public class PlayerToMatchConnector : IDisposable
  {
    private const float LocationOffset = 30.0f;
    
    private readonly NetworkManager _networkManager;
    private readonly MatchesState _matchesState;
    private readonly MatchmakingService _matchmakingService;
    private readonly AppScopeState _appScopeState;
    private readonly MatchFactory _matchFactory;
    private readonly ServerMessageSender _messageSender;
    private readonly ServerMessageReceiver _messageReceiver;


    public PlayerToMatchConnector(NetworkManager networkManager, MatchesState matchesState,
      MatchmakingService matchmakingService, AppScopeState appScopeState,
      ServerMessageSender messageSender, ServerMessageReceiver messageReceiver, MatchFactory matchFactory)
    {
      _networkManager = networkManager;
      _matchesState = matchesState;
      _matchmakingService = matchmakingService;
      _appScopeState = appScopeState;
      _messageSender = messageSender;
      _messageReceiver = messageReceiver;
      _matchFactory = matchFactory;

      _messageReceiver.RegisterMessageHandler<AttachToMatchRequest>(Network_OnAttachToMatchRequested);
    }

    public void Dispose()
    {
      _messageReceiver.UnregisterMessageHandler<AttachToMatchRequest>();
    }

    private async void Network_OnAttachToMatchRequested(ulong senderId, AttachToMatchRequest message)
    {
      // todokmo проверять еще serverId
      var roomId = await _matchmakingService.GetRoomIdForUserAsync(message.UserSecret);
      
      // игроку не разрешено подключаться к комнате
      if (roomId != message.MatchId)
      {
        _networkManager.DisconnectClient(senderId);
        return;
      }
      
      if (!_matchesState.Matches.TryGet(roomId, out var matchState))
      {
        var locationId = await _matchmakingService.GetLocationIdForRoomAsync(roomId);
        var locationOffset = Vector3.up * _matchesState.Matches.Count * LocationOffset;
        matchState = _matchFactory.Create(roomId, locationId, locationOffset, _appScopeState.AppScope);
        _matchesState.Matches.Add(roomId, matchState);
      }

      await UniTask.Yield();
      
      _messageSender.Send(new AttachedToMatchCommand(), senderId);
      matchState.Users.Add(message.UserSecret, senderId);
    }
  }
}