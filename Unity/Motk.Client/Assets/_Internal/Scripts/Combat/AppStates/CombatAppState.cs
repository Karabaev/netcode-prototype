using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Motk.Combat.Client.Core;
using Motk.Combat.Client.Core.Network.Server;
using Motk.Combat.Client.gRPC;
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
    private CombatState _combatState = null!;
    
    public override async UniTask EnterAsync(Context context)
    {
      await SceneManager.LoadSceneAsync("Combat");
      _scope = _appScope.CreateChild<CombatScope>();
      _scope.name = "[Combat]";

      _combatState = _scope.Container.Resolve<CombatState>();

      new GrpcClient().Start();
      
      var stateMachine = _scope.Container.Resolve<ApplicationStateMachine>();
      stateMachine.EnterAsync<EnterToCombatAppState>().Forget();
    }
    
    public override UniTask ExitAsync()
    {
      _scope.Dispose();
      return UniTask.CompletedTask;
    }

    private void Network_OnCombatRoundStarted(CombatRoundStartedCommand command)
    {
      _combatState.RoundIndex.Value = command.Index;
      _combatState.FirstPhaseTurnsQueue.Clear();
      foreach (var unitIdDto in command.TurnsQueue)
        _combatState.FirstPhaseTurnsQueue.Add(CombatStatesUtils.FromDto(unitIdDto));
      
      _combatState.SecondPhaseTurnsQueue.Clear();
    }
    
    private void ConfigureScope(IContainerBuilder builder)
    {

    }
    
    public CombatAppState(ApplicationStateMachine stateMachine, LifetimeScope appScope) : base(stateMachine)
    {
      _appScope = appScope;
    }

    public record Context(string CombatId);
  }
}