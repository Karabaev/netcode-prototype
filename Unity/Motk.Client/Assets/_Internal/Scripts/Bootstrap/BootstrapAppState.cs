﻿using System.Threading;
using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Descriptors;
using com.karabaev.descriptors.abstractions.Initialization;
using com.karabaev.descriptors.unity;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Campaign.Client.CameraSystem.Descriptors;
using Motk.Campaign.Client.Player;
using Motk.Combat.Client.AppStates;
using Motk.Combat.Client.Render.Units.Descriptors;
using Motk.Descriptors;
using Motk.Shared.Configuration;
using Motk.Shared.Locations;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Motk.Client.Bootstrap
{
  [UsedImplicitly]
  public class BootstrapAppState : ApplicationState<DummyStateContext>
  {
    private readonly CurrentPlayerState _currentPlayerState;
    private readonly GameCameraConfigRegistry _gameCameraConfigRegistry;
    private readonly LocationsRegistry _locationsRegistry;
    private readonly UnitVisualRegistry _unitVisualRegistry;
    private readonly DescriptorsBootstrapper _descriptorsBootstrapper;

    private CancellationTokenSource _cts = null!;
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      _cts = new CancellationTokenSource();
      Application.targetFrameRate = 60;
      _currentPlayerState.PlayerId = RandomUtils.RandomString();
      await FetchConfigAsync();

      await _descriptorsBootstrapper.BootstrapAsync(_cts.Token);
      
      await LoadDescriptorsAsync();

      var combatContext = new CombatAppState.Context("");
      EnterNextStateAsync<CombatAppState, CombatAppState.Context>(combatContext).Forget();
      
      // var stateContext = new CampaignAppState.Context(_locationsRegistry.Entries.PickRandom().Key);
      // EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(stateContext).Forget();
    }

    public override UniTask ExitAsync()
    {
      _cts.Cancel();
      _cts.Dispose();
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
          _gameCameraConfigRegistry,
          _unitVisualRegistry
        },
        new DescriptorSourceTypes(new []
        {
          typeof(GameCameraConfigSource),
          typeof(UnitVisualRegistrySource)
        }));
      
      return descriptorInitializer.InitializeAsync();
    }
    
    public BootstrapAppState(ApplicationStateMachine stateMachine, CurrentPlayerState currentPlayerState,
      GameCameraConfigRegistry gameCameraConfigRegistry, LocationsRegistry locationsRegistry,
      UnitVisualRegistry unitVisualRegistry, DescriptorsBootstrapper descriptorsBootstrapper) : base(stateMachine)
    {
      _currentPlayerState = currentPlayerState;
      _gameCameraConfigRegistry = gameCameraConfigRegistry;
      _locationsRegistry = locationsRegistry;
      _unitVisualRegistry = unitVisualRegistry;
      _descriptorsBootstrapper = descriptorsBootstrapper;
    }
  }
}