using UnityEngine;

namespace Motk.Client.Combat.Units.Core
{
  public interface ICombatUnitView
  {
    Vector3 Position { set; }
    
    Quaternion Rotation { set; }
  }
}