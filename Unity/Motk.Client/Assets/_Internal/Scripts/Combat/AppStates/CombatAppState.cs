using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.camera.unity.Views;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MessagePipe;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.InputSystem;
using Motk.Client.Combat.Network.Server;
using Motk.Client.Combat.Units.Core.Controllers;
using Motk.Client.Combat.Units.Core.Services;
using Motk.Client.Combat.Units.Render;
using Motk.Client.Core;
using Motk.Client.Core.InputSystem;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.AStar;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Motk.Client.Combat.AppStates
{
  [UsedImplicitly]
  public class CombatAppState : ApplicationState<CombatAppState.Context>
  {
    private readonly LifetimeScope _appScope;
    
    private LifetimeScope _scope = null!;
    private CombatState _combatState = null!;
    
    public override async UniTask EnterAsync(Context context)
    {
      await SceneManager.LoadSceneAsync("Combat");
      _scope = _appScope.CreateChild(ConfigureScope);
      _scope.name = "[Combat]";

      _combatState = _scope.Container.Resolve<CombatState>();
      
      var stateMachine = _scope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<EnterToCombatAppState>().Forget();
    }
    
    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    private void Network_OnCombatRoundStarted(CombatRoundStartedCommand command)
    {
      _combatState.RoundIndex.Value = command.Index;
      _combatState.FirstPhaseTurnsQueue.Clear();
      foreach (var unitIdDto in command.TurnsQueue)
        _combatState.FirstPhaseTurnsQueue.Add(CombatStatesUtils.FromDto(unitIdDto));
      
      _combatState.SecondPhaseTurnsQueue.Clear();
    }
    
    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<CombatState>(Lifetime.Singleton);
      builder.Register<SelfCombatState>(Lifetime.Singleton);
      
      builder.Register<HexGridVisualState>(Lifetime.Singleton);
      builder.Register<HexGrid.Core.HexGrid>(Lifetime.Singleton);
      builder.Register<InputState>(Lifetime.Singleton);
      builder.Register<CombatInputState>(Lifetime.Singleton);
      builder.Register<CombatInputController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      builder.RegisterInstance(Object.FindObjectOfType<InputController>());

      builder.Register<AStarPathFindingService<HexCoordinates>>(Lifetime.Singleton);
      builder.Register<IMapNodeProvider<HexCoordinates>, HexGridMapNodeProvider>(Lifetime.Singleton);
      builder.Register<IHeuristicCalculator<HexCoordinates>, HexHeuristicCalculator>(Lifetime.Singleton);
      
      builder.RegisterInstance(Object.FindObjectOfType<GameCameraView>());

      builder.Register<ICombatUnitFactory, CombatUnitFactory>(Lifetime.Singleton);
      builder.Register<CombatUnitsController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
      
      builder.Register<ApplicationStateMachine>(Lifetime.Singleton);
      builder.Register<ApplicationStateFactory>(Lifetime.Singleton).As<IStateFactory>();
      builder.Register<EnterToCombatAppState>(Lifetime.Transient);
      builder.Register<PlayerTeamMoveCombatAppState>(Lifetime.Transient);
      builder.Register<OtherTeamMoveCombatAppState>(Lifetime.Transient);
      
      builder.RegisterMessagePipe();
    }
    
    public CombatAppState(ApplicationStateMachine stateMachine, LifetimeScope appScope) : base(stateMachine)
    {
      _appScope = appScope;
    }

    public record Context(string CombatId);
  }
}