using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using com.karabaev.camera.unity.States;
using com.karabaev.camera.unity.Views;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Campaign.Client.Transitions;
using Motk.Client.Core.InputSystem;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.Campaign.Client
{
  [UsedImplicitly]
  public class LoadingCampaignAppState : ApplicationState<DummyStateContext>
  {
    private readonly LocationsRegistry _locationsRegistry;
    private readonly InputController _inputController;
    private readonly InputState _inputState;
    private readonly GameCameraView _gameCameraView;
    private readonly GameCameraConfigRegistry _cameraConfigRegistry;
    private readonly GameCameraState _gameCameraState;
    private readonly CampaignState _campaignState;
    private readonly ApplicationStateMachine _globalStateMachine;

    public override UniTask EnterAsync(DummyStateContext context)
    {
      _campaignState.LocationView = CreateLocation(_campaignState.LocationId);
      
      _inputController.Construct(_inputState);
      
      InitializeCamera();
      Object.FindObjectOfType<TransitionDebug>().Construct(_globalStateMachine, _locationsRegistry);
      EnterNextStateAsync<CampaignLoopAppState>().Forget();
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    private GameObject CreateLocation(string locationId)
    {
      var locationDescriptor = _locationsRegistry.Entries[locationId];
      return Object.Instantiate(locationDescriptor.Prefab);
    }
    
    private void InitializeCamera()
    {
      var cameraConfig = _cameraConfigRegistry.RequireSingle();
      _gameCameraView.Construct(cameraConfig, _gameCameraState, _inputState);
    }
    
    public LoadingCampaignAppState(ApplicationStateMachine stateMachine, LifetimeScope parentScope,
      LocationsRegistry locationsRegistry, InputController inputController, InputState inputState,
      GameCameraView gameCameraView, GameCameraConfigRegistry cameraConfigRegistry,
      GameCameraState gameCameraState, CampaignState campaignState) : base(stateMachine)
    {
      _locationsRegistry = locationsRegistry;
      _inputController = inputController;
      _inputState = inputState;
      _gameCameraView = gameCameraView;
      _cameraConfigRegistry = cameraConfigRegistry;
      _gameCameraState = gameCameraState;
      _campaignState = campaignState;
      _globalStateMachine = parentScope.Parent.Container.Resolve<ApplicationStateMachine>();
    }
  }
}