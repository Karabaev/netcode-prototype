using System;
using Motk.CampaignServer.Actors;
using Motk.CampaignServer.Matches;
using Motk.CampaignServer.Matches.States;
using Motk.Matchmaking;
using Motk.Shared.Core;
using Motk.Shared.Locations;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.CampaignServer
{
  public class EntryPoint : MonoBehaviour
  {
    [SerializeField]
    private LocationsRegistry _locationsRegistry = null!;
    
    private LifetimeScope _scope = null!;
    
    private void Awake()
    {
      _scope = LifetimeScope.Create(ConfigureScope);
      _scope.name = "Application";

      _scope.Container.Resolve<AppScopeState>().AppScope = _scope;

      _scope.Container.Resolve<ClientConnectionListener>();
      // _scope.Container.Resolve<SpawnActorsController>(); todokmo в Match
    }

    private void Start()
    {
      _scope.Container.Resolve<NetworkManager>().StartServer();
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(FindObjectOfType<NetworkManager>());
      builder.Register<ClientConnectionListener>(Lifetime.Singleton);
      builder.Register<MatchLifeCycleController>(Lifetime.Singleton);
      builder.RegisterInstance(_locationsRegistry);
      builder.Register<InMemoryPlayerLocationStorage>(Lifetime.Singleton).As<IPlayerLocationStorage>();
      
      builder.Register<MatchmakingService>(Lifetime.Singleton);


      builder.Register<AppScopeState>(Lifetime.Singleton);
      
      builder.Register<MatchesState>(Lifetime.Singleton);
      builder.Register<MatchGarbageCollector>(Lifetime.Singleton).As<ITickable>();
      builder.Register<PlayerToMatchConnector>(Lifetime.Singleton).As<IDisposable>();
    }
  }
}