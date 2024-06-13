using com.karabaev.applicationLifeCycle.StateMachine;
using Motk.Client.Campaign;
using Motk.Client.Campaign.Actors;
using Motk.Client.Core;
using Motk.Client.Core.Input;
using Motk.Matchmaking;
using Motk.Shared.Core;
using Motk.Shared.Locations;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.Client
{
  public class ApplicationEntryPoint : MonoBehaviour
  {
    [SerializeField]
    private LocationsRegistry _locationsRegistry = null!;
    [SerializeField]
    private CharactersRegistry _charactersRegistry = null!;
    
    private void Awake()
    {
      var appScope = LifetimeScope.Create(ConfigureAppScope);
      appScope.name = "Application";

      appScope.Container.Resolve<AppScopeState>().AppScope = appScope;

      var stateMachine = appScope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<ConnectionAppState>();
    }
    
    private void ConfigureAppScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(FindObjectOfType<NetworkManager>());
      builder.Register<AppScopeState>(Lifetime.Singleton);
      builder.Register<InputState>(Lifetime.Singleton);

      RegisterAppStateMachine(builder);
    }

    private void RegisterAppStateMachine(IContainerBuilder builder)
    {
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<ApplicationStateFactory>(Lifetime.Singleton).As<IStateFactory>();

      builder.Register<ConnectionAppState>(Lifetime.Transient);
      builder.Register<CampaignAppState>(Lifetime.Transient);

      builder.RegisterInstance(_locationsRegistry);
      builder.RegisterInstance(_charactersRegistry);
      builder.Register<MatchmakingService>(Lifetime.Singleton);
      
      builder.Register<CurrentPlayerClientState>(Lifetime.Singleton);
    }
  }
}