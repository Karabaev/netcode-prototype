using UnityEngine;

namespace Motk.Client.Campaign.Actor
{
  public class CampaignActorView : MonoBehaviour
  {
    private CampaignActorState _state = null!;
      
    public void Construct(CampaignActorState state)
    {
      _state = state;
      _state.Position.Changed += State_OnPositionChanged;
    }

    private void State_OnPositionChanged(Vector3 oldValue, Vector3 newValue) => transform.position = newValue;
  }
}