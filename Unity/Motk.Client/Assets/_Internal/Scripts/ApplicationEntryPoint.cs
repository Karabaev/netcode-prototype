using com.karabaev.applicationLifeCycle.StateMachine;
using Motk.Client.Campaign;
using Motk.Client.Campaign.Actor;
using Motk.Client.Core;
using Motk.Client.Core.Input;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.Client
{
  public class ApplicationEntryPoint : MonoBehaviour
  {
    private void Awake()
    {
      var appScope = LifetimeScope.Create(ConfigureAppScope);
      appScope.name = "Application";

      appScope.Container.Resolve<ScopeState>().AppScope = appScope;

      var stateMachine = appScope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<ConnectionAppState, ConnectionAppState.Context>(new ConnectionAppState.Context(appScope));
    }

    private void ConfigureAppScope(IContainerBuilder builder)
    {
      builder.RegisterInstance(FindObjectOfType<NetworkManager>());
      builder.Register<ScopeState>(Lifetime.Singleton);
      builder.Register<InputState>(Lifetime.Singleton);

      RegisterAppStateMachine(builder);
    }

    private void RegisterAppStateMachine(IContainerBuilder builder)
    {
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<ApplicationStateFactory>(Lifetime.Singleton).As<IStateFactory>();

      builder.Register<ConnectionAppState>(Lifetime.Transient);
      builder.Register<CampaignAppState>(Lifetime.Transient);
    }
  }
}