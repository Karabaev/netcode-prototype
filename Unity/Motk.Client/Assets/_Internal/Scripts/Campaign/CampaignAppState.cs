using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using com.karabaev.camera.unity.States;
using com.karabaev.camera.unity.Views;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign.Actors.Controllers;
using Motk.Client.Campaign.Actors.Services;
using Motk.Client.Campaign.InputSystem;
using Motk.Client.Campaign.Movement;
using Motk.Client.Core.InputSystem;
using Motk.Shared.Campaign;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Motk.Client.Campaign
{
  [UsedImplicitly]
  public class CampaignAppState : ApplicationState<CampaignAppState.Context>
  {
    private readonly LocationsRegistry _locationsRegistry;
    private readonly AppScopeState _appScopeState;
    private readonly ClientMessageReceiver _messageReceiver;
    private readonly GameCameraConfigRegistry _cameraConfigRegistry;
    
    private LifetimeScope _scope = null!;
    private CampaignActorsState _campaignActorsState = null!;

    private GameObject _locationView = null!;
    private GameCameraView _cameraView = null!;
    
    public override UniTask EnterAsync(Context context)
    {
      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);
      _campaignActorsState = Resolve<CampaignActorsState>();

      _locationView = CreateLocation(context.LocationId);
      _cameraView = CreateCamera();
      Resolve<InputController>().Construct(Resolve<InputState>());
      Resolve<CampaignInputController>().Initialize(_cameraView);

      _messageReceiver.RegisterMessageHandler<LocationStateMessage>(Network_OnLocationStateObtained);
      _messageReceiver.RegisterMessageHandler<PlayerActorSpawnedCommand>(Network_OnActorSpawned);
      _messageReceiver.RegisterMessageHandler<PlayerActorDespawnedCommand>(Network_OnActorDespawned);
      
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      _locationView.DestroyObject();
      _cameraView.DestroyObject();
      _messageReceiver.UnregisterMessageHandler<LocationStateMessage>();
      _messageReceiver.UnregisterMessageHandler<PlayerActorSpawnedCommand>();
      _messageReceiver.UnregisterMessageHandler<PlayerActorDespawnedCommand>();
      return UniTask.CompletedTask;
    }

    private GameObject CreateLocation(string locationId)
    {
      var locationDescriptor = _locationsRegistry.Entries[locationId];
      return Object.Instantiate(locationDescriptor.Prefab);
    }

    private GameCameraView CreateCamera()
    {
      var cameraView = Object.FindObjectOfType<GameCameraView>();
      var cameraConfig = _cameraConfigRegistry.RequireSingle();
      cameraView.Construct(cameraConfig, Resolve<GameCameraState>(), Resolve<InputState>());
      return cameraView;
    }

    private void Network_OnLocationStateObtained(LocationStateMessage message)
    {
      _campaignActorsState.Actors.Clear();
      
      foreach (var actorDto in message.Actors)
      {
        var actorState = new CampaignActorState();
        actorState.Position.Value = actorDto.Position;
        actorState.Rotation.Value = actorDto.Rotation;
        _campaignActorsState.Actors.Add(actorDto.PlayerId, actorState);
      }
    }

    private void Network_OnActorSpawned(PlayerActorSpawnedCommand message)
    {
      var actorState = new CampaignActorState();
      actorState.Position.Value = message.Actor.Position;
      actorState.Rotation.Value = message.Actor.Rotation;
      _campaignActorsState.Actors.Add(message.Actor.PlayerId, actorState);
    }

    private void Network_OnActorDespawned(PlayerActorDespawnedCommand message)
    {
      _campaignActorsState.Actors.Remove(message.PlayerId);
    }
    
    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<InputState>(Lifetime.Singleton).AsSelf().As<ICameraInputState>();
      builder.RegisterInstance(Object.FindObjectOfType<InputController>());

      builder.Register<CampaignInputState>(Lifetime.Singleton);
      builder.RegisterEntryPoint<CampaignInputController>().AsSelf();

      builder.Register<ActorMovementLogic>(Lifetime.Singleton);
      builder.Register<CampaignActorsState>(Lifetime.Singleton);
      builder.RegisterEntryPoint<LocationActorsController>();
      builder.Register<CampaignActorViewFactory>(Lifetime.Singleton);
      
      builder.RegisterEntryPoint<LocationMovementController>();

      builder.Register<GameCameraState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
    }

    private T Resolve<T>() => _scope.Container.Resolve<T>();

    public CampaignAppState(ApplicationStateMachine stateMachine, LocationsRegistry locationsRegistry,
      AppScopeState appScopeState, ClientMessageReceiver messageReceiver, GameCameraConfigRegistry cameraConfigRegistry) : base(stateMachine)
    {
      _locationsRegistry = locationsRegistry;
      _appScopeState = appScopeState;
      _messageReceiver = messageReceiver;
      _cameraConfigRegistry = cameraConfigRegistry;
    }

    public record Context(string LocationId);
  }
}