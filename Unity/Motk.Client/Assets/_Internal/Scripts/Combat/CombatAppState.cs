using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Shared.Core;
using VContainer;
using VContainer.Unity;

namespace Motk.Client.Combat
{
  [UsedImplicitly]
  public class CombatAppState : ApplicationState<CombatAppState.Context>
  {
    private readonly AppScopeState _appScopeState;
    
    private LifetimeScope _scope = null!;
    
    public override UniTask EnterAsync(Context context)
    {
      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<EnterToCombatAppState>(Lifetime.Transient);
      builder.Register<PlayerTeamMoveCombatAppState>(Lifetime.Transient);
      builder.Register<OtherTeamMoveCombatAppState>(Lifetime.Transient);
    }
    
    public CombatAppState(ApplicationStateMachine stateMachine, AppScopeState appScopeState) : base(stateMachine)
    {
      _appScopeState = appScopeState;
    }

    public record Context(string CombatId);
  }
}