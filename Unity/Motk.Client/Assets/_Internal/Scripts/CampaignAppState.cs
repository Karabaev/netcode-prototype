using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using Game.Campaign;
using VContainer;
using VContainer.Unity;

namespace Game
{
  public class CampaignAppState : ApplicationState<DummyStateContext>
  {
    private LifetimeScope _scope = null!;
    
    public CampaignAppState(ApplicationStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override UniTask EnterAsync(DummyStateContext context)
    {
      _scope = Resolve<ScopeState>().AppScope.CreateChild(ConfigureScope);

      Resolve<CampaignInputController>();
      
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<CampaignInputState>(Lifetime.Singleton);
      builder.Register<CampaignInputController>(Lifetime.Singleton);
    }

    private T Resolve<T>() => _scope.Container.Resolve<T>();
  }
}