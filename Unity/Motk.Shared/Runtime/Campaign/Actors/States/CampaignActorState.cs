using com.karabaev.reactivetypes.Property;
using JetBrains.Annotations;
using UnityEngine;

namespace Motk.Shared.Campaign.Actors.States
{
  [UsedImplicitly]
  public class CampaignActorState
  {
    // todokmo В сторе позиция должна будет храниться без Offset, а в геймплее с ним
    public ReactiveProperty<Vector3> Position { get; } 
    
    public ReactiveProperty<float> EulerY { get; }

    public CampaignActorState(Vector3 position, float eulerY)
    {
      Position = new ReactiveProperty<Vector3>(position);
      EulerY = new ReactiveProperty<float>(eulerY);
    }
  }
}