using System.Collections.Generic;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.InputSystem;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.AStar;
using UnityEngine;

namespace Motk.Client.Combat
{
  [UsedImplicitly]
  public class PlayerTeamMoveCombatAppState : ApplicationState<DummyStateContext>
  {
    private readonly CombatInputState _combatInputState;
    private readonly AStarPathFindingService<HexCoordinates> _pathFindingService;

    private GameObject _pawn = null!;
    
    public override UniTask EnterAsync(DummyStateContext context)
    {
      _pawn = GameObject.Find("Pawn");

      _combatInputState.HexClicked.Invoked += State_OnHexClicked;
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _combatInputState.HexClicked.Invoked -= State_OnHexClicked;
      return UniTask.CompletedTask;
    }

    private void State_OnHexClicked(HexCoordinates clickedHex)
    {
      var origin = _pawn.transform.position.ToAxialCoordinates();
      var path = _pathFindingService.FindPath(origin, clickedHex);
      if (path.Count == 0)
        return;
      
      MoveAsync(path).Forget();
    }

    private async UniTaskVoid MoveAsync(Stack<HexCoordinates> path)
    {
      while (path.TryPop(out var nextCoordinates))
      {
        await UniTask.Delay(200);
        _pawn.transform.position = nextCoordinates.ToWorld(0.0f);
      }
    }

    public PlayerTeamMoveCombatAppState(ApplicationStateMachine stateMachine, CombatInputState combatInputState,
      AStarPathFindingService<HexCoordinates> pathFindingService) : base(stateMachine)
    {
      _combatInputState = combatInputState;
      _pathFindingService = pathFindingService;
    }
  }
}