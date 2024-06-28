using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using com.karabaev.descriptors.abstractions.Initialization;
using com.karabaev.descriptors.unity;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign.CameraSystem.Descriptors;
using Motk.Client.Campaign.Player;
using Motk.Client.Connection;
using UnityEngine;

namespace Motk.Client
{
  [UsedImplicitly]
  public class BootstrapAppState : ApplicationState<DummyStateContext>
  {
    private readonly CurrentPlayerState _currentPlayerState;
    private readonly GameCameraConfigRegistry _gameCameraConfigRegistry;

    public override async UniTask EnterAsync(DummyStateContext context)
    {
      Application.targetFrameRate = 60;
      _currentPlayerState.PlayerId = RandomUtils.RandomString();
      await LoadDescriptorsAsync();

      var stateContext = new EnterToLocationAppState.Context("default");
      EnterNextStateAsync<EnterToLocationAppState, EnterToLocationAppState.Context>(stateContext).Forget();
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    private UniTask LoadDescriptorsAsync()
    {
      var descriptorInitializer = new DescriptorInitializer(new IDescriptorSourceProvider[]
        {
          new ResourcesDescriptorSourceProvider(),
          new DummyDescriptorSourceProvider(),
        },
        new IMutableDescriptorRegistry[]
        {
          _gameCameraConfigRegistry
        },
        new DescriptorSourceTypes(new []{ typeof(GameCameraConfigSource) }));
      
      return descriptorInitializer.InitializeAsync();
    }
    
    public BootstrapAppState(ApplicationStateMachine stateMachine, CurrentPlayerState currentPlayerState, GameCameraConfigRegistry gameCameraConfigRegistry) : base(stateMachine)
    {
      _currentPlayerState = currentPlayerState;
      _gameCameraConfigRegistry = gameCameraConfigRegistry;
    }
  }
}