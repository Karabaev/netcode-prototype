using Motk.Shared.Campaign.Actors.States;
using UnityEngine;

namespace Motk.Client.Campaign.Actors.Views
{
  public class CampaignActorView : MonoBehaviour
  {
    private CampaignActorState _state = null!;
      
    public void Construct(CampaignActorState state)
    {
      _state = state;
      _state.Position.Changed += State_OnPositionChanged;
      _state.EulerY.Changed += State_OnEulerYChanged;
    }

    private void OnDestroy()
    {
      if (_state == null!)
        return;
      
      _state.Position.Changed -= State_OnPositionChanged;
      _state.EulerY.Changed -= State_OnEulerYChanged;
    }

    private void State_OnPositionChanged(Vector3 oldValue, Vector3 newValue) => transform.position = newValue;

    private void State_OnEulerYChanged(float oldValue, float newValue) => transform.rotation = Quaternion.Euler(Vector3.up * newValue);
  }
}