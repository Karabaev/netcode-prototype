using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.Client
{
  [UsedImplicitly]
  public class CampaignAppState : ApplicationState<CampaignAppState.Context>
  {
    private LifetimeScope _scope = null!;

    public override UniTask EnterAsync(Context context)
    {
      _scope = context.ParentScope.CreateChild(ConfigureScope);

      Resolve<CampaignInputController>();
      
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<CampaignInputState>(Lifetime.Singleton);
      builder.Register<CampaignInputController>(Lifetime.Singleton);

      builder.RegisterInstance(Camera.main!);
    }

    private T Resolve<T>() => _scope.Container.Resolve<T>();

    public CampaignAppState(ApplicationStateMachine stateMachine) : base(stateMachine)
    {
    }

    public record Context(LifetimeScope ParentScope);
  }
}