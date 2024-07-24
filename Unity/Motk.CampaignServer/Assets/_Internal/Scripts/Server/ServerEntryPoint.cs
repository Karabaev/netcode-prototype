using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using Motk.CampaignServer.DebugSystem;
using Motk.CampaignServer.Match;
using Motk.CampaignServer.Matchmaking;
using Motk.CampaignServer.Server.Net;
using Motk.CampaignServer.Server.States;
using Motk.Shared.Campaign.PathFinding;
using Motk.Shared.Configuration;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Locations;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.CampaignServer.Server
{
  public class ServerEntryPoint : MonoBehaviour
  {
    [SerializeField]
    private LocationsRegistry _locationsRegistry = null!;
    
    private LifetimeScope _scope = null!;

    private float _nextUpdateTime;
    
    private void Awake()
    {
      Debug.Log("Game server is starting...");

      Application.targetFrameRate = 30;
      Physics.simulationMode = SimulationMode.Script;
      Physics2D.simulationMode = SimulationMode2D.Script;
      
      _scope = LifetimeScope.Create(ConfigureScope);
      _scope.name = "Application";

      _scope.Container.Resolve<AppScopeState>().AppScope = _scope;
      FindObjectOfType<ActorsGizmos>().Construct(_scope.Container);
      FindObjectOfType<ServerDebugUIView>().Construct(_scope.Container.Resolve<ServerState>());
    }

    private void Start() => StartServerAsync().Forget();

    private async UniTaskVoid StartServerAsync()
    {
      await FetchConfigAsync();
      
      var config = _scope.Container.Resolve<IConfig>();
      StartServer(config.GameServerHost, config.GameServerPort);
      
      _scope.Container.Resolve<RemoveMatchController>().Initialize();
      _scope.Container.Resolve<PlayerToMatchRouter>();
      _scope.Container.Resolve<ClientConnectionListener>();
    }

    private async UniTask FetchConfigAsync()
    {
      await UnityServices.InitializeAsync();

      if (!AuthenticationService.Instance.IsSignedIn)
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

      RemoteConfigService.Instance.SetEnvironmentID("c954e02e-40c1-4e1e-b049-b374b838d17d");
      
      await RemoteConfigService.Instance.FetchConfigsAsync(new RemoteConfigUserAttributes(), new RemoteConfigAppAttributes());
    }

    [SuppressMessage("ReSharper", "RedundantAssignment")]
    private void StartServer(string host, int port)
    {
      var transport = (UnityTransport)_scope.Container.Resolve<NetworkManager>().NetworkConfig.NetworkTransport;

#if UNITY_EDITOR
      host = transport.ConnectionData.ServerListenAddress;
      port = transport.ConnectionData.Port;
#endif
      
      transport.SetConnectionData(string.Empty, (ushort) port, host);
      _scope.Container.Resolve<NetworkManager>().StartServer();
      Debug.Log($"Game server started. {host}:{port}...");
    }
    
    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(FindObjectOfType<NetworkManager>());
      builder.Register<ClientConnectionListener>(Lifetime.Singleton);
      builder.RegisterInstance(_locationsRegistry);
      
      builder.Register<MatchmakingClient>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      builder.Register<RemoveMatchController>(Lifetime.Singleton);

      builder.Register<AppScopeState>(Lifetime.Singleton);
      
      builder.Register<ServerState>(Lifetime.Singleton);
      builder.Register<PlayerToMatchRouter>(Lifetime.Singleton);
      builder.Register<MatchFactory>(Lifetime.Singleton);
      builder.Register<ServerMessageSender>(Lifetime.Singleton);
      builder.Register<ServerMessageReceiver>(Lifetime.Singleton);
      builder.Register<MessageSerializer>(Lifetime.Singleton);
        
      builder.Register<NavMeshPathFindingService>(Lifetime.Singleton);

      builder.Register<IConfig, UnityRemoteConfig>(Lifetime.Singleton);
    }
  }
}