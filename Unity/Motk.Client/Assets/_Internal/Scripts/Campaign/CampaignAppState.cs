using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.States;
using com.karabaev.camera.unity.Views;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign.Actors.Controllers;
using Motk.Client.Campaign.Actors.Services;
using Motk.Client.Campaign.InputSystem;
using Motk.Client.Campaign.Movement;
using Motk.Client.Core;
using Motk.Client.Core.InputSystem;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Campaign.Movement;
using Motk.Shared.Core;
using Motk.Shared.Core.Net;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Motk.Client.Campaign
{
  [UsedImplicitly]
  public class CampaignAppState : ApplicationState<CampaignAppState.Context>
  {
    private readonly AppScopeState _appScopeState;

    private LifetimeScope _scope = null!;
    
    public override async UniTask EnterAsync(Context context)
    {
      await SceneManager.LoadSceneAsync("Campaign");
      
      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);
      _scope.Container.Resolve<CampaignState>().LocationId = context.LocationId;
      var stateMachine = _scope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<ConnectToCampaignAppState>().Forget();
    }

    public override UniTask ExitAsync()
    {
      _scope.Container.Resolve<NetworkManager>().Shutdown(true);

      var locationView = _scope.Container.Resolve<CampaignState>().LocationView;
      if (locationView)
        locationView!.DestroyObject();
      
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<CampaignState>(Lifetime.Singleton);
      builder.Register<CampaignActorsState>(Lifetime.Singleton);

      builder.RegisterInstance(Object.FindObjectOfType<InputController>());
      builder.Register<InputState>(Lifetime.Singleton).AsSelf().As<ICameraInputState>();
      
      builder.Register<CampaignInputState>(Lifetime.Singleton);
      builder.Register<AutomatedCampaignInputController>(Lifetime.Singleton);
      builder.Register<ManualCampaignInputController>(Lifetime.Singleton);
      
      builder.Register<GameCameraState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      builder.RegisterInstance(Object.FindObjectOfType<GameCameraView>());

      builder.Register<ActorMovementLogic>(Lifetime.Singleton);
      builder.Register<CampaignActorViewFactory>(Lifetime.Singleton);

      builder.RegisterEntryPoint<LocationActorsController>();

      builder.Register<LocationMovementController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      
      builder.RegisterInstance(Object.FindObjectOfType<NetworkManager>());
      builder.Register<ClientMessageSender>(Lifetime.Singleton);
      builder.Register<ClientMessageReceiver>(Lifetime.Singleton);
      builder.Register<MessageSerializer>(Lifetime.Singleton);
      
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<IStateFactory, ApplicationStateFactory>(Lifetime.Singleton);
      builder.Register<ConnectToCampaignAppState>(Lifetime.Transient);
      builder.Register<LoadingCampaignAppState>(Lifetime.Transient);
      builder.Register<CampaignLoopAppState>(Lifetime.Transient);
    }
    
    public CampaignAppState(ApplicationStateMachine stateMachine, AppScopeState appScopeState) : base(stateMachine)
    {
      _appScopeState = appScopeState;
    }

    public record Context(string LocationId);
  }
}