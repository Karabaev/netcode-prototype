using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Netcode;
using VContainer.Unity;

namespace Motk.Client
{
  [UsedImplicitly]
  public class ConnectionAppState : ApplicationState<ConnectionAppState.Context>
  {
    private readonly NetworkManager _networkManager;

    public override UniTask EnterAsync(Context context)
    {
      _networkManager.StartClient();
      EnterNextStateAsync<CampaignAppState, CampaignAppState.Context>(new CampaignAppState.Context(context.ParentScope))
        .Forget();
      return UniTask.CompletedTask;
    }

    public override UniTask ExitAsync()
    {
      return UniTask.CompletedTask;
    }

    public ConnectionAppState(ApplicationStateMachine stateMachine, NetworkManager networkManager) : base(stateMachine)
    {
      _networkManager = networkManager;
    }

    public record Context(LifetimeScope ParentScope);
  }
}