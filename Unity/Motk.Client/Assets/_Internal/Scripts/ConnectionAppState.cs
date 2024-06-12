using System;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Matchmaking;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Motk.Client
{
  [UsedImplicitly]
  public class ConnectionAppState : ApplicationState<DummyStateContext>
  {
    private const string ConnectingLocationId = "default";
    
    private readonly NetworkManager _networkManager;
    private readonly MatchmakingService _matchmakingService;

    public override async UniTask EnterAsync(DummyStateContext context)
    {
      var ticketId = await _matchmakingService.CreateTicketAsync("Player1", ConnectingLocationId);
      var connectionParameters = await PollTicketAsync(ticketId);
      
      var transport = (UnityTransport)_networkManager.NetworkConfig.NetworkTransport;
      transport.SetConnectionData(connectionParameters.Host, connectionParameters.Port);
      _networkManager.StartClient();
      _networkManager.OnConnectionEvent += OnConnectedToCampaignServer;
    }

    private void OnConnectedToCampaignServer(NetworkManager _, ConnectionEventData evt)
    {
      if (evt.EventType != ConnectionEvent.ClientConnected)
        return;

      var context = new CampaignAppState.Context(ConnectingLocationId);
      EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(context).Forget();
    }
    
    public override UniTask ExitAsync()
    {
      _networkManager.OnConnectionEvent -= OnConnectedToCampaignServer;
      return UniTask.CompletedTask;
    }

    private async UniTask<ConnectionParameters> PollTicketAsync(Guid ticketId)
    {
      while (true)
      {
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        var ticketStatus = await _matchmakingService.GetTicketStatusAsync(ticketId);

        if (ticketStatus.TicketStatus == TicketStatus.Found)
        {
          return ticketStatus.ConnectionParameters!;
        }

        if (ticketStatus.TicketStatus == TicketStatus.Failed)
        {
          throw new NotImplementedException("To matchmaking failed state");
        }
      }
    }

    public ConnectionAppState(ApplicationStateMachine stateMachine, NetworkManager networkManager,
      MatchmakingService matchmakingService) : base(stateMachine)
    {
      _networkManager = networkManager;
      _matchmakingService = matchmakingService;
    }
  }
}