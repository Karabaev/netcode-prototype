using UnityEngine;

namespace Motk.Combat.Client.Core.Units
{
  public interface ICombatUnitView
  {
    Vector3 Position { set; }
    
    Quaternion Rotation { set; }
  }
}