using Motk.CampaignServer.Locations;
using Motk.Matchmaking;
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

      _scope.Container.Resolve<ClientConnectionListener>();
      _scope.Container.Resolve<RoomLocationController>();
    }

    private void Start()
    {
      _scope.Container.Resolve<NetworkManager>().StartServer();
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(FindObjectOfType<NetworkManager>());
      builder.Register<ClientConnectionListener>(Lifetime.Singleton);
      builder.Register<RoomLocationController>(Lifetime.Singleton);
      builder.RegisterInstance(_locationsRegistry);
      builder.Register<CampaignLocationsState>(Lifetime.Singleton);
      builder.Register<InMemoryPlayerLocationStorage>(Lifetime.Singleton).As<IPlayerLocationStorage>();
      
      builder.Register<MatchmakingService>(Lifetime.Singleton);
    }
  }
}