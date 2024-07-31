using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;

namespace Motk.Client.Combat.AppStates
{
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