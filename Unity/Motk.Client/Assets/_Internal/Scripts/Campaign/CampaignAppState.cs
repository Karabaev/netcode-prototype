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
using Motk.Client.Campaign.Transitions;
using Motk.Client.Core.InputSystem;
using Motk.Shared.Campaign;
using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Motk.Shared.Locations;
using Unity.Netcode;
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
    private readonly ApplicationStateMachine _applicationStateMachine;
    private readonly NetworkManager _networkManager;
    
    private LifetimeScope _scope = null!;
    private CampaignActorsState _campaignActorsState = null!;

    private GameObject _locationView = null!;
    
    public override UniTask EnterAsync(Context context)
    {
      Debug.Log("Campaign started...");

      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);
      _campaignActorsState = Resolve<CampaignActorsState>();

      _locationView = CreateLocation(context.LocationId);
      InitializeCamera();
      Resolve<InputController>().Construct(Resolve<InputState>());
      Resolve<ManualCampaignInputController>();
      Resolve<AutomatedCampaignInputController>();
      Object.FindObjectOfType<TransitionDebug>().Construct(_applicationStateMachine);

      _messageReceiver.RegisterMessageHandler<LocationStateMessage>(Network_OnLocationStateObtained);
      _messageReceiver.RegisterMessageHandler<PlayerActorSpawnedCommand>(Network_OnActorSpawned);
      _messageReceiver.RegisterMessageHandler<PlayerActorDespawnedCommand>(Network_OnActorDespawned);
      
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      _locationView.DestroyObject();
      _messageReceiver.UnregisterMessageHandler<LocationStateMessage>();
      _messageReceiver.UnregisterMessageHandler<PlayerActorSpawnedCommand>();
      _messageReceiver.UnregisterMessageHandler<PlayerActorDespawnedCommand>();
      _networkManager.Shutdown(true);
      return UniTask.CompletedTask;
    }

    private GameObject CreateLocation(string locationId)
    {
      var locationDescriptor = _locationsRegistry.Entries[locationId];
      return Object.Instantiate(locationDescriptor.Prefab);
    }

    private void InitializeCamera()
    {
      var cameraView = Resolve<GameCameraView>();
      var cameraConfig = _cameraConfigRegistry.RequireSingle();
      cameraView.Construct(cameraConfig, Resolve<GameCameraState>(), Resolve<InputState>());
    }

    private void Network_OnLocationStateObtained(LocationStateMessage message)
    {
      _campaignActorsState.Actors.Clear();
      
      foreach (var actorDto in message.Actors)
      {
        var actorState = new CampaignActorState(actorDto.Position, actorDto.EulerY);
        _campaignActorsState.Actors.Add(actorDto.PlayerId, actorState);
      }
    }

    private void Network_OnActorSpawned(PlayerActorSpawnedCommand message)
    {
      var actorState = new CampaignActorState(message.Actor.Position, message.Actor.EulerY);
      _campaignActorsState.Actors.Add(message.Actor.PlayerId, actorState);
    }

    private void Network_OnActorDespawned(PlayerActorDespawnedCommand message)
    {
      _campaignActorsState.Actors.Remove(message.PlayerId);
    }

    private T Resolve<T>() => _scope.Container.Resolve<T>();

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<CampaignActorsState>(Lifetime.Singleton);
      builder.Register<InputState>(Lifetime.Singleton).AsSelf().As<ICameraInputState>();
      builder.Register<CampaignInputState>(Lifetime.Singleton);
      builder.Register<GameCameraState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();

      builder.Register<ActorMovementLogic>(Lifetime.Singleton);
      builder.Register<CampaignActorViewFactory>(Lifetime.Singleton);

      builder.RegisterInstance(Object.FindObjectOfType<InputController>());
      builder.RegisterEntryPoint<LocationActorsController>();

      builder.Register<AutomatedCampaignInputController>(Lifetime.Singleton);
      builder.Register<ManualCampaignInputController>(Lifetime.Singleton);

      
      // if (Application.isBatchMode)
      // {
        // builder.RegisterEntryPoint<AutomatedCampaignInputController>();
      // }
      // else
      // {
        // builder.RegisterEntryPoint<ManualCampaignInputController>().AsSelf();
      // }
      
      builder.RegisterEntryPoint<LocationMovementController>();
      builder.RegisterInstance(Object.FindObjectOfType<GameCameraView>());
    }

    public CampaignAppState(ApplicationStateMachine stateMachine, LocationsRegistry locationsRegistry,
      AppScopeState appScopeState, ClientMessageReceiver messageReceiver, GameCameraConfigRegistry cameraConfigRegistry,
      ApplicationStateMachine applicationStateMachine, NetworkManager networkManager) : base(stateMachine)
    {
      _locationsRegistry = locationsRegistry;
      _appScopeState = appScopeState;
      _messageReceiver = messageReceiver;
      _cameraConfigRegistry = cameraConfigRegistry;
      _applicationStateMachine = applicationStateMachine;
      _networkManager = networkManager;
    }

    public record Context(string LocationId);
  }
}