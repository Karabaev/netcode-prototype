using Motk.Shared.Campaign.Actors;
using Motk.Shared.Campaign.Actors.States;
using UnityEngine;

namespace Motk.Client.Campaign.Actors
{
  public class CampaignActorView : MonoBehaviour
  {
    private CampaignActorState _state = null!;
      
    public void Construct(CampaignActorState state)
    {
      _state = state;
      _state.Position.Changed += State_OnPositionChanged;
      _state.Rotation.Changed += State_OnRotationChanged;
    }

    private void State_OnPositionChanged(Vector3 oldValue, Vector3 newValue) => transform.position = newValue;

    private void State_OnRotationChanged(Quaternion oldValue, Quaternion newValue) => transform.rotation = newValue;
  }
}