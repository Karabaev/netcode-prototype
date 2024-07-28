using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using com.karabaev.descriptors.abstractions.Initialization;
using com.karabaev.descriptors.unity;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign;
using Motk.Client.Campaign.CameraSystem.Descriptors;
using Motk.Client.Campaign.Player;
using Motk.Shared.Configuration;
using Motk.Shared.Locations;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Motk.Client
{
  [UsedImplicitly]
  public class BootstrapAppState : ApplicationState<DummyStateContext>
  {
    private readonly CurrentPlayerState _currentPlayerState;
    private readonly GameCameraConfigRegistry _gameCameraConfigRegistry;
    private readonly LocationsRegistry _locationsRegistry;

    public override async UniTask EnterAsync(DummyStateContext context)
    {
      Application.targetFrameRate = 60;
      _currentPlayerState.PlayerId = RandomUtils.RandomString();
      await FetchConfigAsync();
      await LoadDescriptorsAsync();

      // var combatContext = new CombatAppState.Context("");
      // EnterNextStateAsync<CombatAppState, CombatAppState.Context>(combatContext).Forget();
      
      var stateContext = new CampaignAppState.Context(_locationsRegistry.Entries.PickRandom().Key);
      EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(stateContext).Forget();
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    private async UniTask FetchConfigAsync()
    {
      await UnityServices.InitializeAsync();

      if (!AuthenticationService.Instance.IsSignedIn)
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

      RemoteConfigService.Instance.SetEnvironmentID("c954e02e-40c1-4e1e-b049-b374b838d17d");
      
      await RemoteConfigService.Instance.FetchConfigsAsync(new RemoteConfigUserAttributes(), new RemoteConfigAppAttributes());
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
    
    public BootstrapAppState(ApplicationStateMachine stateMachine, CurrentPlayerState currentPlayerState,
      GameCameraConfigRegistry gameCameraConfigRegistry, LocationsRegistry locationsRegistry) : base(stateMachine)
    {
      _currentPlayerState = currentPlayerState;
      _gameCameraConfigRegistry = gameCameraConfigRegistry;
      _locationsRegistry = locationsRegistry;
    }
  }
}