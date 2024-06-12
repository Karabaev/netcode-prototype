using com.karabaev.applicationLifeCycle.StateMachine;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Client.Campaign;
using Motk.Shared.Locations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Motk.Client
{
  [UsedImplicitly]
  public class CampaignAppState : ApplicationState<CampaignAppState.Context>
  {
    private readonly LocationsRegistry _locationsRegistry;
    private readonly AppScopeState _appScopeState;
    private LifetimeScope _scope = null!;

    private GameObject _location = null!;
    
    public override UniTask EnterAsync(Context context)
    {
      _scope = _appScopeState.AppScope.CreateChild(ConfigureScope);

      Resolve<CampaignInputController>();

      var locationDescriptor = _locationsRegistry.Locations[context.LocationId];
      _location = Object.Instantiate(locationDescriptor.Prefab);

      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      _location.DestroyObject();
      return UniTask.CompletedTask;
    }

    private void ConfigureScope(IContainerBuilder builder)
    {
      builder.Register<CampaignInputState>(Lifetime.Singleton);
      builder.Register<CampaignInputController>(Lifetime.Singleton);

      builder.RegisterInstance(Camera.main!);
    }

    private T Resolve<T>() => _scope.Container.Resolve<T>();

    public CampaignAppState(ApplicationStateMachine stateMachine, LocationsRegistry locationsRegistry,
      AppScopeState appScopeState) : base(stateMachine)
    {
      _locationsRegistry = locationsRegistry;
      _appScopeState = appScopeState;
    }

    public record Context(string LocationId);
  }
}