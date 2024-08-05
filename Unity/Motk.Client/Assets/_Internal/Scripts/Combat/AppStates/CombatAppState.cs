using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Motk.Combat.Client.AppStates
{
  [UsedImplicitly]
  public class CombatAppState : ApplicationState<CombatAppState.Context>
  {
    private readonly LifetimeScope _appScope;
    
    private CombatScope _scope = null!;
    
    public override async UniTask EnterAsync(Context context)
    {
      await SceneManager.LoadSceneAsync("Combat");
      _scope = _appScope.CreateChild<CombatScope>();
      _scope.name = "[Combat]";

      var stateMachine = _scope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<EnterToCombatAppState>().Forget();
    }
    
    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    public CombatAppState(ApplicationStateMachine stateMachine, LifetimeScope appScope) : base(stateMachine)
    {
      _appScope = appScope;
    }

    public record Context(string CombatId);
  }
}