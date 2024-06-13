using System;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign;
using Motk.Client.Core;
using Motk.Matchmaking;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using VContainer;
using VContainer.Unity;

namespace Motk.Client.Connection
{
  [UsedImplicitly]
  public class ConnectionAppState : ApplicationState<DummyStateContext>
  {
    private const string ConnectingLocationId = "default";
    private const string PlayerId = "Player1";
    
    private readonly NetworkManager _networkManager;
    private readonly MatchmakingService _matchmakingService;
    private readonly CurrentPlayerClientState _currentPlayerClientState;
    private readonly ClientMessageSender _clientMessageSender;
    private readonly AppScopeState _appScopeState;
    
    private LifetimeScope _scope;

    private string _userSecret = null!;
    private int _matchId = -1;
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);
      
      var ticketId = await _matchmakingService.CreateTicketAsync(PlayerId, ConnectingLocationId);
      var ticketResponse = await PollTicketAsync(ticketId);

      _userSecret = ticketResponse.UserSecret;
      _matchId = ticketResponse.RoomId;
      
      var transport = (UnityTransport)_networkManager.NetworkConfig.NetworkTransport;
      transport.SetConnectionData(ticketResponse.ConnectionParameters!.Host, ticketResponse.ConnectionParameters.Port);
      _networkManager.OnClientConnectedCallback += OnConnectedToServer;
      _networkManager.StartClient();
    }
    
    private void OnConnectedToServer(ulong clientId)
    {
      _currentPlayerClientState.ClientId = clientId;

      _scope.Container.Resolve<AttachedToMatchCommandReceiver>().Initialize(OnAttachedToMatch);
      
      _clientMessageSender.Send(new AttachToMatchRequest
      {
        MatchId = _matchId,
        UserSecret = _userSecret
      });
    }

    private void OnAttachedToMatch()
    {
      var context = new CampaignAppState.Context(ConnectingLocationId);
      EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(context).Forget();
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      _networkManager.OnClientConnectedCallback -= OnConnectedToServer;
      return UniTask.CompletedTask;
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

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<AttachedToMatchCommandReceiver>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
    }
    
    public ConnectionAppState(ApplicationStateMachine stateMachine, NetworkManager networkManager,
      MatchmakingService matchmakingService, CurrentPlayerClientState currentPlayerClientState,
      ClientMessageSender clientMessageSender, AppScopeState appScopeState, LifetimeScope scope) : base(stateMachine)
    {
      _networkManager = networkManager;
      _matchmakingService = matchmakingService;
      _currentPlayerClientState = currentPlayerClientState;
      _clientMessageSender = clientMessageSender;
      _appScopeState = appScopeState;
      _scope = scope;
    }
    
    [UsedImplicitly]
    private class AttachedToMatchCommandReceiver : MessageReceiver<AttachedToMatchCommand>
    {
      private Action _callback = null!;
      
      public AttachedToMatchCommandReceiver(NetworkManager networkManager) : base(networkManager)
      {
      }

      public void Initialize(Action callback) => _callback = callback;

      protected override void OnMessageReceived(ulong senderId, AttachedToMatchCommand message) => _callback.Invoke();
    }
  }
}