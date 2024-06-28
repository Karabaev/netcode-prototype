using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.CampaignServer.Match;
using Motk.CampaignServer.Matchmaking;
using Motk.CampaignServer.Server.Net;
using Motk.CampaignServer.Server.States;
using Motk.Matchmaking;
using Motk.Shared.Core;
using Motk.Shared.Matches;
using Unity.Netcode;
using UnityEngine;

namespace Motk.CampaignServer.Server
{
  [UsedImplicitly]
  public class PlayerToMatchRouter : IDisposable
  {
    private const float LocationOffset = 30.0f;
    
    private readonly NetworkManager _networkManager;
    private readonly ServerState _serverState;
    private readonly MatchmakingClient _matchmakingClient;
    private readonly AppScopeState _appScopeState;
    private readonly MatchFactory _matchFactory;
    private readonly ServerMessageSender _messageSender;
    private readonly ServerMessageReceiver _messageReceiver;

    public PlayerToMatchRouter(NetworkManager networkManager, ServerState serverState,
      MatchmakingClient matchmakingClient, AppScopeState appScopeState,
      ServerMessageSender messageSender, ServerMessageReceiver messageReceiver, MatchFactory matchFactory)
    {
      _networkManager = networkManager;
      _serverState = serverState;
      _matchmakingClient = matchmakingClient;
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

    private async void Network_OnAttachToMatchRequested(ulong clientId, AttachToMatchRequest message)
    {
      // todokmo проверять еще serverId
      var matchId = await _matchmakingClient.GetRoomIdForUserAsync(message.UserSecret);
      
      // игроку не разрешено подключаться к комнате
      if (matchId != message.MatchId)
      {
        _networkManager.DisconnectClient(clientId);
        return;
      }
      
      if (!_serverState.Matches.TryGet(matchId, out var matchState))
      {
        var locationId = await _matchmakingClient.GetLocationIdForRoomAsync(matchId);
        var locationOffset = Vector3.up * _serverState.Matches.Count * LocationOffset;
        matchState = _matchFactory.Create(matchId, locationId, locationOffset, _appScopeState.AppScope);
        _serverState.Matches.Add(matchId, matchState);
      }

      // ожидание, чтобы контроллеры проинициализировались
      await UniTask.Yield();
      
      _messageSender.Send(new AttachedToMatchCommand(), clientId);

      matchState.Users.Add(message.UserSecret, clientId);
      _serverState.ClientsInMatches.Add(clientId, matchId);
    }
  }
}