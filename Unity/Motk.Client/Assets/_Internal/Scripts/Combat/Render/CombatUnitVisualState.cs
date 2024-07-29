using com.karabaev.reactivetypes.Property;
using UnityEngine;

namespace Motk.Client.Combat.Render
{
  public class CombatUnitVisualState
  {
    public ReactiveProperty<Vector3> Position { get; }
    
    public ReactiveProperty<Quaternion> Rotation { get; }

    public CombatUnitVisualState(Vector3 position, Quaternion rotation)
    {
      Position = new ReactiveProperty<Vector3>(position);
      Rotation = new ReactiveProperty<Quaternion>(rotation);
    }
  }
}