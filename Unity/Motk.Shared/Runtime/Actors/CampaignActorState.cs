using com.karabaev.reactivetypes.Property;
using JetBrains.Annotations;
using UnityEngine;

namespace Motk.Shared.Actors
{
  [UsedImplicitly]
  public class CampaignActorState
  {
    public ReactiveProperty<Vector3> Position { get; } = new(Vector3.zero);
    
    public ReactiveProperty<Quaternion> Rotation { get; } = new(Quaternion.identity);
  }
}