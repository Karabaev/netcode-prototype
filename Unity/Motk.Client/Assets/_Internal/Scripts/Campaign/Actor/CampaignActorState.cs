using com.karabaev.reactivetypes.Property;
using UnityEngine;

namespace Game.Campaign.Actor
{
  public class CampaignActorState
  {
    public ReactiveProperty<Vector3> Position { get; } = new(Vector3.zero);
    
    public ReactiveProperty<Quaternion> Rotation { get; } = new(Quaternion.identity);
  }
}