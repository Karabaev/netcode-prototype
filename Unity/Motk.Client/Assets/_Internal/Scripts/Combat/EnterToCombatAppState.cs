using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;

namespace Motk.Client.Combat
{
  public class EnterToCombatAppState : ApplicationState<DummyStateContext>
  {
    public override UniTask EnterAsync(DummyStateContext context)
    {
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    public EnterToCombatAppState(ApplicationStateMachine stateMachine) : base(stateMachine)
    {
    }
  }

  public class PlayerTeamMoveCombatAppState : ApplicationState<DummyStateContext>
  {
    public override UniTask EnterAsync(DummyStateContext context)
    {
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    public PlayerTeamMoveCombatAppState(ApplicationStateMachine stateMachine) : base(stateMachine)
    {
    }
  }

  public class OtherTeamMoveCombatAppState : ApplicationState<DummyStateContext>
  {
    public override UniTask EnterAsync(DummyStateContext context)
    {
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    public OtherTeamMoveCombatAppState(ApplicationStateMachine stateMachine) : base(stateMachine)
    {
    }
  }
}