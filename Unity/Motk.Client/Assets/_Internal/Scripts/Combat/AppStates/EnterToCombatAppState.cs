using System.Collections.Generic;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using JetBrains.Annotations;
using MagicOnion.Client;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.gRPC.Motk.Combat.Server.gRPC;
using Motk.Client.Combat.InputSystem;
using Motk.Client.Combat.Network;
using Motk.Client.Combat.Network.Server;
using Motk.Client.Combat.Units.Core.Controllers;
using Motk.Client.Core.InputSystem;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Motk.Client.Combat.AppStates
{
  [UsedImplicitly]
  public class EnterToCombatAppState : ApplicationState<DummyStateContext>
  {
    private readonly HexGridVisualState _gridVisualState;
    private readonly HexGrid.Core.HexGrid _grid;
    private readonly InputState _inputState;
    private readonly CombatInputController _combatInputController;
    private readonly InputController _inputController;
    private readonly CombatState _combatState;
    private readonly SelfCombatState _selfCombatState;
    private readonly CombatUnitsController _combatUnitsController;

    private readonly ServerMock _serverMock = new();
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
      {
        HttpHandler = new YetAnotherHttpHandler { Http2Only = true }
      });
      var client = MagicOnionClient.Create<IMyFirstService>(channel);
      var result = await client.SumAsync(123, 456);
      Debug.Log($"Result={result}");
      
      // connecting to the server

      _selfCombatState.TeamId = await _serverMock.GetSelfTeamIdAsync();
      
      _inputController.Construct(_inputState);
      _combatInputController.Start();
      _grid.Initialize(CreateMapDescriptor());
      Object.FindObjectOfType<HexGridView>().Construct(_gridVisualState, _grid);

      var combatStateMessage = await _serverMock.GetCombatStateAsync();
      InitializeState(combatStateMessage);

      _combatUnitsController.Start();
      EnterNextStateAsync<PlayerTeamMoveCombatAppState>().Forget();
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    private void InitializeState(CombatStateMessage combatStateMessage)
    {
      _combatState.RoundIndex.Value = combatStateMessage.RoundIndex;
      
      _combatState.FirstPhaseTurnsQueue.Clear();
      foreach (var unitIdDto in combatStateMessage.TurnsQueue)
        _combatState.FirstPhaseTurnsQueue.Add(CombatStatesUtils.FromDto(unitIdDto));

      _combatState.Teams.Clear();
      foreach (var (teamId, teamDto) in combatStateMessage.Teams)
        _combatState.Teams.Add(teamId, CombatStatesUtils.FromDto(teamId, teamDto));
    }
    
    private HexMapDescriptor CreateMapDescriptor()
    {
      var nodes = new List<HexMapNodeDescriptor>
      {
        new(new HexCoordinates(-3, 6), true),
        new(new HexCoordinates(-2, 6), true),
        new(new HexCoordinates(-1, 6), true),
        new(new HexCoordinates(0, 6), true),
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
    
    public EnterToCombatAppState(ApplicationStateMachine stateMachine, HexGridVisualState gridVisualState,
      HexGrid.Core.HexGrid grid, InputState inputState, CombatInputController combatInputController,
      InputController inputController, CombatState combatState,
      SelfCombatState selfCombatState, CombatUnitsController combatUnitsController) : base(stateMachine)
    {
      _gridVisualState = gridVisualState;
      _grid = grid;
      _inputState = inputState;
      _combatInputController = combatInputController;
      _inputController = inputController;
      _combatState = combatState;
      _selfCombatState = selfCombatState;
      _combatUnitsController = combatUnitsController;
    }
  }
}