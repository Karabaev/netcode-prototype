using System.Collections.Generic;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using JetBrains.Annotations;
using Mork.HexGrid.Render.Unity;
using Mork.HexGrid.Render.Unity.Functions;
using Motk.Client.Core.InputSystem;
using Motk.Combat.Client.Core;
using Motk.Combat.Client.Core.InputSystem;
using Motk.Combat.Client.Core.Network;
using Motk.Combat.Client.Core.Units.Controllers;
using Motk.Combat.Client.gRPC;
using Motk.Combat.Shared.Messages.Dto;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Motk.Combat.Client.AppStates
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
    private readonly ICombatMessageSender _combatMessageSender;
    private readonly ICombatMessageReceiver _combatMessageReceiver;
    private readonly GrpcChannelState _grpcChannelState;
    private readonly IHexGridFunctions _hexGridFunctions;
    
    public override async UniTask EnterAsync(DummyStateContext context)
    {
      // todokmo get connection data from matchmaking
      _grpcChannelState.GrpcChannel = GrpcChannel.ForAddress("https://localhost:7037", new GrpcChannelOptions
      {
        HttpHandler = new YetAnotherHttpHandler
        { 
          Http2Only = true,
          SkipCertificateVerification = true
        }
      });
      
      await _combatMessageSender.ConnectAsync();
      _combatMessageReceiver.TeamJoined += Network_OnTeamJoined;
      _combatMessageReceiver.TeamLeft += Network_OnTeamLeft;

      _combatUnitsController.Start();

      _inputController.Construct(_inputState);
      _combatInputController.Start();
      _grid.Initialize(CreateMapDescriptor());
      Object.FindObjectOfType<HexGridView>().Construct(_gridVisualState, _grid, _hexGridFunctions);
      
      _selfCombatState.TeamId = await _combatMessageSender.JoinRoomAsync("roomId", "secret");
      EnterNextStateAsync<PrepareCombatAppState>().Forget();
    }
    
    public override UniTask ExitAsync()
    {
      _combatMessageReceiver.TeamJoined -= Network_OnTeamJoined;
      _combatMessageReceiver.TeamLeft -= Network_OnTeamLeft;
      return UniTask.CompletedTask;
    }

    private void Network_OnTeamJoined(in CombatTeamDto payload)
    {
      var newTeam = CombatStatesUtils.FromDto(in payload);
      _combatState.Teams.Add(payload.TeamId, newTeam);
    }

    private void Network_OnTeamLeft(in ushort teamId)
    {
      _combatState.Teams.Remove(teamId);
    }
    
    private IReadOnlyList<HexMapNodeDescriptor> CreateMapDescriptor()
    {
      var nodes = new List<HexMapNodeDescriptor>
      {
        // new(new HexCoordinates(-3, 6), true),
        // new(new HexCoordinates(-2, 6), true),
        // new(new HexCoordinates(-1, 6), true),
        // new(new HexCoordinates(0, 6), true),
        // new(new HexCoordinates(1, 6), true),
        // new(new HexCoordinates(2, 6), true),
        // new(new HexCoordinates(3, 6), true),
        //
        // new(new HexCoordinates(-2, 5), true),
        // new(new HexCoordinates(-1, 5), true),
        // new(new HexCoordinates(0, 5), true),
        // new(new HexCoordinates(1, 5), true),
        // new(new HexCoordinates(2, 5), true),
        // new(new HexCoordinates(3, 5), true),
        //
        // new(new HexCoordinates(-2, 4), true),
        // new(new HexCoordinates(-1, 4), true),
        // new(new HexCoordinates(0, 4), true),
        // new(new HexCoordinates(1, 4), true),
        // new(new HexCoordinates(2, 4), true),
        // new(new HexCoordinates(3, 4), true),
        // new(new HexCoordinates(4, 4), true),
        //
        // new(new HexCoordinates(-1, 3), true),
        // new(new HexCoordinates(0, 3), true),
        // new(new HexCoordinates(1, 3), true),
        // new(new HexCoordinates(2, 3), true),
        // new(new HexCoordinates(3, 3), true),
        // new(new HexCoordinates(4, 3), true),
        //
        // new(new HexCoordinates(-1, 2), true),
        // new(new HexCoordinates(0, 2), true),
        // new(new HexCoordinates(1, 2), true),
        // new(new HexCoordinates(2, 2), true),
        // new(new HexCoordinates(3, 2), true),
        // new(new HexCoordinates(4, 2), true),
        // new(new HexCoordinates(5, 2), true),
        //
        // new(new HexCoordinates(0, 1), true),
        // new(new HexCoordinates(1, 1), true),
        // new(new HexCoordinates(2, 1), true),
        // new(new HexCoordinates(3, 1), true),
        // new(new HexCoordinates(4, 1), true),
        // new(new HexCoordinates(5, 1), true),
        //
        // new(new HexCoordinates(0, 0), true),
        // new(new HexCoordinates(1, 0), true),
        // new(new HexCoordinates(2, 0), true),
        // new(new HexCoordinates(3, 0), false),
        // new(new HexCoordinates(4, 0), false),
        // new(new HexCoordinates(5, 0), false),
        // new(new HexCoordinates(6, 0), true),
        //
        // new(new HexCoordinates(1, -1), true),
        // new(new HexCoordinates(2, -1), true),
        // new(new HexCoordinates(3, -1), true),
        // new(new HexCoordinates(4, -1), true),
        // new(new HexCoordinates(5, -1), true),
        // new(new HexCoordinates(6, -1), true),
        //
        // new(new HexCoordinates(1, -2), true),
        // new(new HexCoordinates(2, -2), true),
        // new(new HexCoordinates(3, -2), true),
        // new(new HexCoordinates(4, -2), true),
        // new(new HexCoordinates(5, -2), true),
        // new(new HexCoordinates(6, -2), true),
        // new(new HexCoordinates(7, -2), true),
        //
        // new(new HexCoordinates(2, -3), true),
        // new(new HexCoordinates(3, -3), true),
        // new(new HexCoordinates(4, -3), true),
        // new(new HexCoordinates(5, -3), true),
        // new(new HexCoordinates(6, -3), true),
        // new(new HexCoordinates(7, -3), true)
      };
      return nodes;
    }
    
    public EnterToCombatAppState(ApplicationStateMachine stateMachine, HexGridVisualState gridVisualState,
      HexGrid.Core.HexGrid grid, InputState inputState, CombatInputController combatInputController,
      InputController inputController, CombatState combatState,
      SelfCombatState selfCombatState, CombatUnitsController combatUnitsController,
      ICombatMessageSender combatMessageSender, GrpcChannelState grpcChannelState,
      ICombatMessageReceiver combatMessageReceiver, IHexGridFunctions hexGridFunctions) : base(stateMachine)
    {
      _gridVisualState = gridVisualState;
      _grid = grid;
      _inputState = inputState;
      _combatInputController = combatInputController;
      _inputController = inputController;
      _combatState = combatState;
      _selfCombatState = selfCombatState;
      _combatUnitsController = combatUnitsController;
      _combatMessageSender = combatMessageSender;
      _grpcChannelState = grpcChannelState;
      _combatMessageReceiver = combatMessageReceiver;
      _hexGridFunctions = hexGridFunctions;
    }
  }
}