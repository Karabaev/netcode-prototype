using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using Motk.Client.Connection;
using UnityEngine;

namespace Motk.Client.Campaign.Transitions
{
  public class TransitionDebug : MonoBehaviour
  {
    private ApplicationStateMachine _applicationStateMachine = null!;

    private void Awake() => enabled = false;

    public void Construct(ApplicationStateMachine applicationStateMachine)
    {
      _applicationStateMachine = applicationStateMachine;
      enabled = true;
    }
    
    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.F))
      {
        var stateContext = new EnterToLocationAppState.Context("second");
        _applicationStateMachine.EnterAsync<EnterToLocationAppState, EnterToLocationAppState.Context>(stateContext).Forget();
      }

      if (Input.GetKeyDown(KeyCode.G))
      {
        var stateContext = new EnterToLocationAppState.Context("default");
        _applicationStateMachine.EnterAsync<EnterToLocationAppState, EnterToLocationAppState.Context>(stateContext).Forget();
      }
    }
  }
}