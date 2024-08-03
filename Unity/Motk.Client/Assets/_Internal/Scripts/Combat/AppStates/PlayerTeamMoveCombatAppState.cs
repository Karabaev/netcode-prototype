using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Core.InputSystem;
using Motk.Combat.Client.Core;
using Motk.Combat.Client.Core.InputSystem;
using Motk.Combat.Client.Core.Network;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.AStar;

namespace Motk.Combat.Client.AppStates
{
  [UsedImplicitly]
  public class PlayerTeamMoveCombatAppState : ApplicationState<DummyStateContext>
  {
    private readonly CombatState _combatState;
    private readonly CombatInputState _combatInputState;
    private readonly AStarPathFindingService<HexCoordinates> _pathFindingService;
    private readonly HexGridVisualState _gridVisualState;
    private readonly InputState _inputState;
    private readonly ServerMock _serverMock;

    public override UniTask EnterAsync(DummyStateContext context)
    {
      _combatInputState.HexClicked.Invoked += Input_OnHexClicked;
      _inputState.DefendRaised.Invoked += Input_OnDefendRaised;
      _inputState.WaitRaised.Invoked += Input_OnWaitRaised;
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _combatInputState.HexClicked.Invoked -= Input_OnHexClicked;
      _inputState.DefendRaised.Invoked -= Input_OnDefendRaised;
      _inputState.WaitRaised.Invoked -= Input_OnWaitRaised;
      return UniTask.CompletedTask;
    }

    private void Input_OnHexClicked(HexCoordinates clickedHex)
    {
      var activeUnitIdentifier = _combatState.FirstPhaseTurnsQueue[0];
      var activeUnitState = _combatState.RequireUnit(activeUnitIdentifier);

      var origin = activeUnitState.Position.Value;
      var path = _pathFindingService.FindPath(origin, clickedHex);
      if (path.Count == 0)
        return;
      
      activeUnitState.MoveIntended.Invoke(path);
    }

    private void Input_OnDefendRaised()
    {
      _serverMock.SendDefendActionAsync().Forget();
      
      _combatState.FirstPhaseTurnsQueue.RemoveAt(0);
    }

    private void Input_OnWaitRaised()
    {
      _serverMock.SendWaitActionAsync().Forget();
      
      var unit = _combatState.FirstPhaseTurnsQueue[0];
      _combatState.FirstPhaseTurnsQueue.RemoveAt(0);
      _combatState.SecondPhaseTurnsQueue.Add(unit);
    }

    public PlayerTeamMoveCombatAppState(ApplicationStateMachine stateMachine, CombatInputState combatInputState,
      AStarPathFindingService<HexCoordinates> pathFindingService, HexGridVisualState gridVisualState,
      CombatState combatState, InputState inputState, ServerMock serverMock) : base(stateMachine)
    {
      _combatInputState = combatInputState;
      _pathFindingService = pathFindingService;
      _gridVisualState = gridVisualState;
      _combatState = combatState;
      _inputState = inputState;
      _serverMock = serverMock;
    }
  }
}