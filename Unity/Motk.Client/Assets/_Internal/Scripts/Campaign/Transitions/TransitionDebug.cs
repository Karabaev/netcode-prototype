using System.Linq;
using com.karabaev.applicationLifeCycle.StateMachine;
using Cysharp.Threading.Tasks;
using Motk.Shared.Locations;
using UnityEngine;

namespace Motk.Campaign.Client.Transitions
{
  public class TransitionDebug : MonoBehaviour
  {
    private ApplicationStateMachine _applicationStateMachine = null!;
    private LocationsRegistry _locationsRegistry = null!;

    private void Awake() => enabled = false;

    public void Construct(ApplicationStateMachine applicationStateMachine, LocationsRegistry locationsRegistry)
    {
      _applicationStateMachine = applicationStateMachine;
      _locationsRegistry = locationsRegistry;
      enabled = true;
    }
    
    private void Update()
    {
      var locationId = string.Empty;
      if (Input.GetKeyDown(KeyCode.F))
      {
         locationId = _locationsRegistry.Entries.ToList()[0].Key;
      } else if (Input.GetKeyDown(KeyCode.G))
      {
        locationId = _locationsRegistry.Entries.ToList()[1].Key;
      }

      if (string.IsNullOrEmpty(locationId))
        return;

      var stateContext = new CampaignAppState.Context(locationId);
      _applicationStateMachine.EnterAsync<CampaignAppState, CampaignAppState.Context>(stateContext).Forget();
    }
  }
}