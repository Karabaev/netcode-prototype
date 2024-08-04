using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Combat.Client.Core;
using Motk.Combat.Client.Core.InputSystem;
using Motk.Combat.Client.Core.Network;
using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Client.AppStates
{
  [UsedImplicitly]
  public class PrepareCombatAppState : ApplicationState<DummyStateContext>
  {
    private readonly ICombatMessageReceiver _combatMessageReceiver;
    private readonly ICombatMessageSender _combatMessageSender;
    private readonly CombatState _combatState;
    private readonly CombatInputState _combatInputState;

    public override UniTask EnterAsync(DummyStateContext context)
    {
      _combatMessageReceiver.TeamJoined += Network_OnTeamJoined;
      _combatMessageReceiver.TeamLeft += Network_OnTeamLeft;
      _combatMessageReceiver.CombatStarted += NetworkOnCombatStarted;
      _combatInputState.ReadyToBattleRaised.Invoked += Input_OnReadyToBattleRaised;
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _combatMessageReceiver.TeamJoined -= Network_OnTeamJoined;
      _combatMessageReceiver.TeamLeft -= Network_OnTeamLeft;
      _combatMessageReceiver.CombatStarted -= NetworkOnCombatStarted;
      _combatInputState.ReadyToBattleRaised.Invoked -= Input_OnReadyToBattleRaised;
      return UniTask.CompletedTask;
    }

    private void Network_OnTeamJoined(in CombatTeamDto payload)
    {
      var newTeam = CombatStatesUtils.FromDto(in payload);
      _combatState.Teams.Add(payload.TeamId, newTeam);
    }

    private void Network_OnTeamLeft(in ushort teamId) => _combatState.Teams.Remove(teamId);

    private void NetworkOnCombatStarted() => EnterNextStateAsync<PlayerTeamMoveCombatAppState>().Forget();

    private void Input_OnReadyToBattleRaised()
    {
      _combatMessageSender.ReadyAsync().Forget();
      // todokmo change ui state
    }

    public PrepareCombatAppState(ApplicationStateMachine stateMachine, ICombatMessageReceiver combatMessageReceiver,
      ICombatMessageSender combatMessageSender, CombatState combatState, CombatInputState combatInputState) : base(stateMachine)
    {
      _combatMessageReceiver = combatMessageReceiver;
      _combatMessageSender = combatMessageSender;
      _combatState = combatState;
      _combatInputState = combatInputState;
    }
  }
}