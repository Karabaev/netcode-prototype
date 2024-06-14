using com.karabaev.reactivetypes.Property;
using JetBrains.Annotations;
using UnityEngine;

namespace Motk.Shared.Campaign.Actors.States
{
  [UsedImplicitly]
  public class CampaignActorState
  {
    // todokmo В сторе позиция должна будет храниться без Offset, а в геймплее с ним
    public ReactiveProperty<Vector3> Position { get; } = new(Vector3.zero);
    
    public ReactiveProperty<Quaternion> Rotation { get; } = new(Quaternion.identity);
  }
}