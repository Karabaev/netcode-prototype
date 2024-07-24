using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using Cysharp.Threading.Tasks;
using Motk.Client.Campaign;
using Motk.Client.Campaign.Actors.Descriptors;
using Motk.Client.Campaign.Player;
using Motk.Client.Combat;
using Motk.Client.Connection;
using Motk.Client.Core;
using Motk.Client.Core.InputSystem;
using Motk.Client.Matchmaking;
using Motk.Shared.Configuration;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
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
      Debug.Log("Bootstrap started...");
      var appScope = LifetimeScope.Create(ConfigureAppScope);
      appScope.name = "Application";

      appScope.Container.Resolve<AppScopeState>().AppScope = appScope;
      
      var stateMachine = appScope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<BootstrapAppState>().Forget();
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

      builder.Register<BootstrapAppState>(Lifetime.Transient);
      builder.Register<EnterToLocationAppState>(Lifetime.Transient);
      builder.Register<CampaignAppState>(Lifetime.Transient);
      builder.Register<CombatAppState>(Lifetime.Transient);

      builder.RegisterInstance(_locationsRegistry);
      builder.RegisterInstance(_charactersRegistry);
      builder.Register<MatchmakingClient>(Lifetime.Singleton);
      
      builder.Register<CurrentPlayerState>(Lifetime.Singleton);
      
      builder.Register<ClientMessageSender>(Lifetime.Singleton);
      builder.Register<ClientMessageReceiver>(Lifetime.Singleton);
      builder.Register<MessageSerializer>(Lifetime.Singleton);
      
      builder.Register<GameCameraConfigRegistry>(Lifetime.Singleton);
      
      builder.Register<IConfig, UnityRemoteConfig>(Lifetime.Singleton);
    }
  }
}