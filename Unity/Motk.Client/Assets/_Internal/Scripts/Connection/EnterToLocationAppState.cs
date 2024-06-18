using System;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign;
using Motk.Client.Campaign.Player;
using Motk.Matchmaking;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Motk.Client.Connection
{
  [UsedImplicitly]
  public class EnterToLocationAppState : ApplicationState<EnterToLocationAppState.Context>
  {
    private readonly NetworkManager _networkManager;
    private readonly MatchmakingService _matchmakingService;
    private readonly CurrentPlayerState _currentPlayerState;
    private readonly ClientMessageSender _clientMessageSender;
    private readonly ClientMessageReceiver _messageReceiver;

    private string _userSecret = null!; // todokmo возможно заменить на TicketId?
    private int _matchId = -1;
    private string _locationId = null!;
    
    public override async UniTask EnterAsync(Context context)
    {
      _locationId = context.LocationId;
      var ticketId = await _matchmakingService.CreateTicketAsync(_currentPlayerState.PlayerId, context.LocationId);
      var ticketResponse = await PollTicketAsync(ticketId);

      _userSecret = ticketResponse.UserSecret;
      _matchId = ticketResponse.RoomId;
      
      var transport = (UnityTransport)_networkManager.NetworkConfig.NetworkTransport;
      transport.SetConnectionData(ticketResponse.ConnectionParameters!.Host, ticketResponse.ConnectionParameters.Port);
      _networkManager.OnConnectionEvent += OnConnectionChanged;
      _networkManager.StartClient();
    }

    public override UniTask ExitAsync()
    {
      _networkManager.OnConnectionEvent -= OnConnectionChanged;
      _messageReceiver.UnregisterMessageHandler<AttachedToMatchCommand>();
      return UniTask.CompletedTask;
    }
    
    private void OnConnectionChanged(NetworkManager _, ConnectionEventData connectionData)
    {
      if (connectionData.EventType != ConnectionEvent.ClientConnected)
        return;

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
      var context = new CampaignAppState.Context(_locationId);
      EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(context).Forget();
    }

    private async UniTask<TicketStatusResponse> PollTicketAsync(Guid ticketId)
    {
      while (true)
      {
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        var ticketStatus = await _matchmakingService.GetTicketStatusAsync(ticketId);

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

    public EnterToLocationAppState(ApplicationStateMachine stateMachine, NetworkManager networkManager,
      MatchmakingService matchmakingService, CurrentPlayerState currentPlayerState,
      ClientMessageSender clientMessageSender,
      ClientMessageReceiver messageReceiver) : base(stateMachine)
    {
      _networkManager = networkManager;
      _matchmakingService = matchmakingService;
      _currentPlayerState = currentPlayerState;
      _clientMessageSender = clientMessageSender;
      _messageReceiver = messageReceiver;
    }

    public record Context(string LocationId);
  }
}