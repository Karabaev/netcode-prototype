using Motk.CampaignServer.DebugSystem;
using Motk.CampaignServer.Match;
using Motk.CampaignServer.Server.Net;
using Motk.CampaignServer.Server.States;
using Motk.Matchmaking;
using Motk.Shared.Campaign.PathFinding;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Locations;
using Unity.Netcode;
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

    private MatchmakingService _matchmakingService = null!;
    private float _nextUpdateTime;
    
    private void Awake()
    {
      Application.targetFrameRate = 30;
      _scope = LifetimeScope.Create(ConfigureScope);
      _scope.name = "Application";

      _scope.Container.Resolve<AppScopeState>().AppScope = _scope;

      FindObjectOfType<ActorsGizmos>().Construct(_scope.Container);
      
      _scope.Container.Resolve<ClientConnectionListener>();

      _matchmakingService = _scope.Container.Resolve<MatchmakingService>();
    }

    private void Start()
    {
      _scope.Container.Resolve<NetworkManager>().StartServer();
      _scope.Container.Resolve<PlayerToMatchRouter>();
      _matchmakingService.InitializeStorage();
    }

    private void Update()
    {
      if (Time.time < _nextUpdateTime)
        return;

      _nextUpdateTime += 1.0f;
      _matchmakingService.Update();
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(FindObjectOfType<NetworkManager>());
      builder.Register<ClientConnectionListener>(Lifetime.Singleton);
      builder.RegisterInstance(_locationsRegistry);
      
      builder.Register<MatchmakingService>(Lifetime.Singleton);
      builder.Register<MatchmakingStorage>(Lifetime.Singleton);
      builder.RegisterEntryPoint<RemoveMatchController>();

      builder.Register<AppScopeState>(Lifetime.Singleton);
      
      builder.Register<ServerState>(Lifetime.Singleton);
      builder.Register<PlayerToMatchRouter>(Lifetime.Singleton);
      builder.Register<MatchFactory>(Lifetime.Singleton);
      builder.Register<ServerMessageSender>(Lifetime.Singleton);
      builder.Register<ServerMessageReceiver>(Lifetime.Singleton);
      builder.Register<MessageSerializer>(Lifetime.Singleton);
        
      builder.Register<NavMeshPathFindingService>(Lifetime.Singleton);
    }
  }
}