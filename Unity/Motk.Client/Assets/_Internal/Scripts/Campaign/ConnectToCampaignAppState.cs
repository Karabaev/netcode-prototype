using System;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign.Player;
using Motk.Client.Matchmaking;
using Motk.Matchmaking;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Motk.Client.Campaign
{
  [UsedImplicitly]
  public class ConnectToCampaignAppState : ApplicationState<DummyStateContext>
  {
    private readonly NetworkManager _networkManager;
    private readonly MatchmakingClient _matchmakingClient;
    private readonly CurrentPlayerState _currentPlayerState;
    private readonly ClientMessageSender _clientMessageSender;
    private readonly ClientMessageReceiver _messageReceiver;
    private readonly CampaignState _campaignState;

    private string _userSecret = null!; // todokmo возможно заменить на TicketId?
    private int _matchId = -1;
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      Debug.Log($"Enter to location started. UserId={_currentPlayerState.PlayerId}, LocationId={_campaignState.LocationId}");
      var ticketId = await _matchmakingClient.CreateTicketAsync(_currentPlayerState.PlayerId, _campaignState.LocationId);
      var ticketResponse = await PollTicketAsync(ticketId);

      _userSecret = ticketResponse.UserSecret;
      _matchId = ticketResponse.RoomId;
      
      StartConnectToServer(ticketResponse.ConnectionParameters!.Host, ticketResponse.ConnectionParameters.Port);
    }
    
    public override UniTask ExitAsync()
    {
      _networkManager.OnConnectionEvent -= OnConnectionChanged;
      _messageReceiver.UnregisterMessageHandler<AttachedToMatchCommand>();
      return UniTask.CompletedTask;
    }
    
    private void StartConnectToServer(string host, ushort port)
    {
      Debug.Log($"Connecting to game server {host}:{port}...");

      var transport = (UnityTransport)_networkManager.NetworkConfig.NetworkTransport;
      transport.SetConnectionData(host, port);
      _networkManager.OnConnectionEvent += OnConnectionChanged;
      _networkManager.StartClient();
    }
    
    private void OnConnectionChanged(NetworkManager _, ConnectionEventData connectionData)
    {
      if (connectionData.EventType == ConnectionEvent.ClientDisconnected)
      {
        Debug.Log("Disconnected from server");
        return;
      }

      if (connectionData.EventType != ConnectionEvent.ClientConnected)
        return;
      
      Debug.Log("Connected to game server...");

      _currentPlayerState.ClientId = connectionData.ClientId;

      _messageReceiver.RegisterMessageHandler<AttachedToMatchCommand>(OnAttachedToMatch);
      
      _clientMessageSender.Send(new AttachToMatchRequest
      {
        MatchId = _matchId,
        UserSecret = _userSecret
      });
    }

    private void OnAttachedToMatch(AttachedToMatchCommand _)
    {
      EnterNextStateAsync<LoadingCampaignAppState>().Forget();
    }

    private async UniTask<TicketStatusResponse> PollTicketAsync(Guid ticketId)
    {
      while (true)
      {
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        var ticketStatus = await _matchmakingClient.GetTicketStatusAsync(ticketId);

        if (ticketStatus.TicketStatus == TicketStatus.Found)
        {
          return ticketStatus;
        }

        if (ticketStatus.TicketStatus == TicketStatus.Failed)
        {
          throw new NotImplementedException("To matchmaking failed state");
        }
      }
    }

    public ConnectToCampaignAppState(ApplicationStateMachine stateMachine, NetworkManager networkManager,
      MatchmakingClient matchmakingClient, CurrentPlayerState currentPlayerState,
      ClientMessageSender clientMessageSender,
      ClientMessageReceiver messageReceiver, CampaignState campaignState) : base(stateMachine)
    {
      _networkManager = networkManager;
      _matchmakingClient = matchmakingClient;
      _currentPlayerState = currentPlayerState;
      _clientMessageSender = clientMessageSender;
      _messageReceiver = messageReceiver;
      _campaignState = campaignState;
    }
  }
}