using com.karabaev.applicationLifeCycle.StateMachine;
using Game.Campaign;
using Game.Campaign.Actor;
using Game.Core;
using Game.Core.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
  public class ApplicationEntryPoint : MonoBehaviour
  {
    private readonly InputState _inputState = new();
    private readonly CampaignInputState _campaignInputState = new();

    private readonly CampaignActorState _playerActorState = new();

    [SerializeField]
    private CampaignActorView _playerPrefab = null!;
    
    private void Awake()
    {
      var appScope = LifetimeScope.Create(ConfigureAppScope);
      appScope.name = "Application";

      appScope.Container.Resolve<ScopeState>().AppScope = appScope;

      var stateMachine = appScope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<CampaignAppState>();


      // var inputController = FindObjectOfType<InputController>();
      // inputController.Construct(_inputState);
      //
      // var campaignInputController = FindObjectOfType<CampaignInputController>();
      // campaignInputController.Construct(_campaignInputState, _inputState, Camera.main);
      //
      // var actor = Instantiate(_playerPrefab);
      // actor.Construct(_playerActorState);
    }

    private void ConfigureAppScope(IContainerBuilder builder)
    {
      builder.Register<ScopeState>(Lifetime.Singleton);
      builder.Register<InputState>(Lifetime.Singleton);

      RegisterAppStateMachine(builder);
    }

    private void RegisterAppStateMachine(IContainerBuilder builder)
    {
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<ApplicationStateFactory>(Lifetime.Singleton).As<IStateFactory>();

      builder.Register<CampaignAppState>(Lifetime.Transient);
    }
  }
}