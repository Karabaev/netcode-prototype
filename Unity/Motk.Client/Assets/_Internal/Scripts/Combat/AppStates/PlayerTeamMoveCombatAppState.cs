using System;
using System.Collections.Generic;
using System.Linq;
using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.InputSystem;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.AStar;
using UnityEngine;

namespace Motk.Client.Combat.AppStates
{
  [UsedImplicitly]
  public class PlayerTeamMoveCombatAppState : ApplicationState<DummyStateContext>
  {
    private readonly CombatInputState _combatInputState;
    private readonly AStarPathFindingService<HexCoordinates> _pathFindingService;
    private readonly HexGridVisualState _gridVisualState;

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
      _gridVisualState.VisibleNodes.Clear();
      while (path.TryPop(out var nextCoordinates))
      {
        var randomNodeVisual = Enum
          .GetValues(typeof(GridNodeVisualStateType))
          .OfType<GridNodeVisualStateType>()
          .Where(t => t != GridNodeVisualStateType.None)
          .PickRandom();
        _gridVisualState.VisibleNodes.Add(nextCoordinates, randomNodeVisual);
        await UniTask.Delay(200);
        _pawn.transform.position = nextCoordinates.ToWorld(0.0f);
      }
    }

    public PlayerTeamMoveCombatAppState(ApplicationStateMachine stateMachine, CombatInputState combatInputState,
      AStarPathFindingService<HexCoordinates> pathFindingService, HexGridVisualState gridVisualState) : base(stateMachine)
    {
      _combatInputState = combatInputState;
      _pathFindingService = pathFindingService;
      _gridVisualState = gridVisualState;
    }
  }
}