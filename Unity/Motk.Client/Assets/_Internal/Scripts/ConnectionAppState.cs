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
    private readonly NetworkManager _networkManager;
    private readonly AppScopeState _appScopeState;
    private readonly MatchmakingService _matchmakingService;

    public override async UniTask EnterAsync(DummyStateContext context)
    {
      var ticketId = await _matchmakingService.CreateTicketAsync("Player1", "default");
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
      
      EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(new CampaignAppState.Context(_appScopeState.AppScope))
        .Forget();
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
      AppScopeState appScopeState, MatchmakingService matchmakingService) : base(stateMachine)
    {
      _networkManager = networkManager;
      _appScopeState = appScopeState;
      _matchmakingService = matchmakingService;
    }
  }
}