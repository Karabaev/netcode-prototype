using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Views;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.InputSystem;
using Motk.Client.Core;
using Motk.Client.Core.InputSystem;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.AStar;
using Motk.Shared.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Motk.Client.Combat
{
  [UsedImplicitly]
  public class CombatAppState : ApplicationState<CombatAppState.Context>
  {
    private readonly AppScopeState _appScopeState;
    
    private LifetimeScope _scope = null!;
    
    public override async UniTask EnterAsync(Context context)
    {
      await SceneManager.LoadSceneAsync("Combat");

      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);
      var stateMachine = _scope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<EnterToCombatAppState>().Forget();
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<HexGridVisualState>(Lifetime.Singleton);
      builder.Register<HexGrid.Core.HexGrid>(Lifetime.Singleton);
      builder.Register<InputState>(Lifetime.Singleton);
      builder.Register<CombatInputState>(Lifetime.Singleton);
      builder.Register<CombatInputController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      builder.Register<AStarPathFindingService<HexCoordinates>>(Lifetime.Singleton);
      builder.Register<IMapNodeProvider<HexCoordinates>, HexGridMapNodeProvider>(Lifetime.Singleton);
      builder.Register<IHeuristicCalculator<HexCoordinates>, HexHeuristicCalculator>(Lifetime.Singleton);
      
      builder.RegisterInstance(Object.FindObjectOfType<GameCameraView>());

      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<ApplicationStateFactory>(Lifetime.Singleton).As<IStateFactory>();
      builder.Register<EnterToCombatAppState>(Lifetime.Transient);
      builder.Register<PlayerTeamMoveCombatAppState>(Lifetime.Transient);
      builder.Register<OtherTeamMoveCombatAppState>(Lifetime.Transient);
    }
    
    public CombatAppState(ApplicationStateMachine stateMachine, AppScopeState appScopeState) : base(stateMachine)
    {
      _appScopeState = appScopeState;
    }

    public record Context(string CombatId);
  }
}