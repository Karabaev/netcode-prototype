using System;
using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using com.karabaev.descriptors.abstractions.Initialization;
using com.karabaev.descriptors.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign;
using Motk.Client.Campaign.CameraSystem.Descriptors;
using Motk.Client.Core;
using Motk.Matchmaking;
using Motk.Shared.Core.Net;
using Motk.Shared.Matches;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

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
    private readonly ClientMessageReceiver _messageReceiver;
    private readonly GameCameraConfigRegistry _gameCameraConfigRegistry;


    private string _userSecret = null!;
    private int _matchId = -1;
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      var loadingTask = LoadDescriptorsAsync();
      
      var ticketId = await _matchmakingService.CreateTicketAsync(PlayerId, ConnectingLocationId);
      var ticketResponse = await PollTicketAsync(ticketId);

      await loadingTask;
      
      _userSecret = ticketResponse.UserSecret;
      _matchId = ticketResponse.RoomId;
      
      var transport = (UnityTransport)_networkManager.NetworkConfig.NetworkTransport;
      transport.SetConnectionData(ticketResponse.ConnectionParameters!.Host, ticketResponse.ConnectionParameters.Port);
      _networkManager.OnConnectionEvent += OnConnectionChanged;
      _networkManager.StartClient();
    }

    public override UniTask ExitAsync()
    {
      _messageReceiver.UnregisterMessageHandler<AttachedToMatchCommand>();
      return UniTask.CompletedTask;
    }

    private UniTask LoadDescriptorsAsync()
    {
      var descriptorInitializer = new DescriptorInitializer(new IDescriptorSourceProvider[]
        {
          new ResourcesDescriptorSourceProvider(),
          new DummyDescriptorSourceProvider(),
        },
        new IMutableDescriptorRegistry[]
        {
          _gameCameraConfigRegistry
        },
        new DescriptorSourceTypes(new []{ typeof(GameCameraConfigSource) }));
      
      return descriptorInitializer.InitializeAsync();
    }

    private void OnConnectionChanged(NetworkManager _, ConnectionEventData connectionData)
    {
      if (connectionData.EventType != ConnectionEvent.ClientConnected)
        return;

      _currentPlayerClientState.ClientId = connectionData.ClientId;

      _messageReceiver.RegisterMessageHandler<AttachedToMatchCommand>(OnAttachedToMatch);
      
      _clientMessageSender.Send(new AttachToMatchRequest
      {
        MatchId = _matchId,
        UserSecret = _userSecret
      });
    }

    private void OnAttachedToMatch(AttachedToMatchCommand _)
    {
      var context = new CampaignAppState.Context(ConnectingLocationId);
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

    public ConnectionAppState(ApplicationStateMachine stateMachine, NetworkManager networkManager,
      MatchmakingService matchmakingService, CurrentPlayerClientState currentPlayerClientState,
      ClientMessageSender clientMessageSender,
      ClientMessageReceiver messageReceiver, GameCameraConfigRegistry gameCameraConfigRegistry) : base(stateMachine)
    {
      _networkManager = networkManager;
      _matchmakingService = matchmakingService;
      _currentPlayerClientState = currentPlayerClientState;
      _clientMessageSender = clientMessageSender;
      _messageReceiver = messageReceiver;
      _gameCameraConfigRegistry = gameCameraConfigRegistry;
    }
  }
}