using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.States;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign.Actors;
using Motk.Client.Campaign.Movement;
using Motk.Client.Core.Input;
using Motk.Shared.Campaign.Actors;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Core;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.Client.Campaign
{
  [UsedImplicitly]
  public class CampaignAppState : ApplicationState<CampaignAppState.Context>
  {
    private readonly LocationsRegistry _locationsRegistry;
    private readonly CharactersRegistry _charactersRegistry;
    private readonly AppScopeState _appScopeState;
    private LifetimeScope _scope = null!;

    private GameObject _location = null!;
    
    public override UniTask EnterAsync(Context context)
    {
      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);

      Resolve<CampaignInputController>();
      Resolve<CampaignActorViewsController>();
      Resolve<InputController>().Construct(Resolve<InputState>());
      Resolve<CampaignMovementEndPoint>();
      
      var locationDescriptor = _locationsRegistry.Entries[context.LocationId];
      _location = Object.Instantiate(locationDescriptor.Prefab);
      
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      _location.DestroyObject();
      return UniTask.CompletedTask;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<InputState>(Lifetime.Singleton);
      builder.Register<InputController>(Lifetime.Singleton);

      builder.Register<CampaignInputState>(Lifetime.Singleton);
      builder.Register<CampaignInputController>(Lifetime.Singleton);

      builder.RegisterInstance(UnityEngine.Camera.main!);
      builder.Register<GameCameraState>(Lifetime.Singleton);

      builder.Register<ActorMovementLogic>(Lifetime.Singleton);
      builder.Register<CampaignActorsState>(Lifetime.Singleton);
      builder.Register<CampaignActorViewsController>(Lifetime.Singleton);
      
      builder.Register<CampaignMovementEndPoint>(Lifetime.Singleton);
    }

    private T Resolve<T>() => _scope.Container.Resolve<T>();

    public CampaignAppState(ApplicationStateMachine stateMachine, LocationsRegistry locationsRegistry,
      AppScopeState appScopeState, CharactersRegistry charactersRegistry) : base(stateMachine)
    {
      _locationsRegistry = locationsRegistry;
      _appScopeState = appScopeState;
      _charactersRegistry = charactersRegistry;
    }

    public record Context(string LocationId);
  }
}