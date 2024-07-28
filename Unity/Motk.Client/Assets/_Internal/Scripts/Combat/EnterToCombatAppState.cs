using System.Collections.Generic;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.InputSystem;
using Motk.Client.Core.InputSystem;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Motk.Client.Combat
{
  [UsedImplicitly]
  public class EnterToCombatAppState : ApplicationState<DummyStateContext>
  {
    private readonly HexGridState _gridState;
    private readonly HexGrid.Core.HexGrid _grid;
    private readonly InputState _inputState;
    private readonly CombatInputController _;
    
    public override UniTask EnterAsync(DummyStateContext context)
    {
      // connecting to the server
      
      _grid.Initialize(CreateMapDescriptor());
      Object.FindObjectOfType<HexGridView>().Construct(_gridState, _grid);
      Object.FindObjectOfType<InputController>().Construct(_inputState);
      EnterNextStateAsync<PlayerTeamMoveCombatAppState>().Forget();
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }
    
    private HexMapDescriptor CreateMapDescriptor()
    {
      var nodes = new List<HexMapNodeDescriptor>
      {
        new(new HexCoordinates(-3, 6), true),
        new(new HexCoordinates(-2, 6), true),
        new(new HexCoordinates(-1, 6), true),
        new(new HexCoordinates(-0, 6), true),
        new(new HexCoordinates(1, 6), true),
        new(new HexCoordinates(2, 6), true),
        new(new HexCoordinates(3, 6), true),

        new(new HexCoordinates(-2, 5), true),
        new(new HexCoordinates(-1, 5), true),
        new(new HexCoordinates(0, 5), true),
        new(new HexCoordinates(1, 5), true),
        new(new HexCoordinates(2, 5), true),
        new(new HexCoordinates(3, 5), true),

        new(new HexCoordinates(-2, 4), true),
        new(new HexCoordinates(-1, 4), true),
        new(new HexCoordinates(0, 4), true),
        new(new HexCoordinates(1, 4), true),
        new(new HexCoordinates(2, 4), true),
        new(new HexCoordinates(3, 4), true),
        new(new HexCoordinates(4, 4), true),

        new(new HexCoordinates(-1, 3), true),
        new(new HexCoordinates(0, 3), true),
        new(new HexCoordinates(1, 3), true),
        new(new HexCoordinates(2, 3), true),
        new(new HexCoordinates(3, 3), true),
        new(new HexCoordinates(4, 3), true),

        new(new HexCoordinates(-1, 2), true),
        new(new HexCoordinates(0, 2), true),
        new(new HexCoordinates(1, 2), true),
        new(new HexCoordinates(2, 2), true),
        new(new HexCoordinates(3, 2), true),
        new(new HexCoordinates(4, 2), true),
        new(new HexCoordinates(5, 2), true),

        new(new HexCoordinates(0, 1), true),
        new(new HexCoordinates(1, 1), true),
        new(new HexCoordinates(2, 1), true),
        new(new HexCoordinates(3, 1), true),
        new(new HexCoordinates(4, 1), true),
        new(new HexCoordinates(5, 1), true),

        new(new HexCoordinates(0, 0), true),
        new(new HexCoordinates(1, 0), true),
        new(new HexCoordinates(2, 0), true),
        new(new HexCoordinates(3, 0), false),
        new(new HexCoordinates(4, 0), false),
        new(new HexCoordinates(5, 0), false),
        new(new HexCoordinates(6, 0), true),

        new(new HexCoordinates(1, -1), true),
        new(new HexCoordinates(2, -1), true),
        new(new HexCoordinates(3, -1), true),
        new(new HexCoordinates(4, -1), true),
        new(new HexCoordinates(5, -1), true),
        new(new HexCoordinates(6, -1), true),

        new(new HexCoordinates(1, -2), true),
        new(new HexCoordinates(2, -2), true),
        new(new HexCoordinates(3, -2), true),
        new(new HexCoordinates(4, -2), true),
        new(new HexCoordinates(5, -2), true),
        new(new HexCoordinates(6, -2), true),
        new(new HexCoordinates(7, -2), true),

        new(new HexCoordinates(2, -3), true),
        new(new HexCoordinates(3, -3), true),
        new(new HexCoordinates(4, -3), true),
        new(new HexCoordinates(5, -3), true),
        new(new HexCoordinates(6, -3), true),
        new(new HexCoordinates(7, -3), true)
      };
      return new HexMapDescriptor(nodes);
    }
    
    public EnterToCombatAppState(ApplicationStateMachine stateMachine, HexGridState gridState,
      HexGrid.Core.HexGrid grid, InputState inputState, CombatInputController _) : base(stateMachine)
    {
      _gridState = gridState;
      _grid = grid;
      _inputState = inputState;
      this._ = _;
    }
  }
}