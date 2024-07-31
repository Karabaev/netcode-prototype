using com.karabaev.utilities.unity.GameKit;
using Motk.Client.Combat.Units.Core;
using UnityEngine;

namespace Motk.Client.Combat.Units
{
  public class CombatUnitView : GameKitComponent, ICombatUnitView
  {
    public Vector3 Position
    {
      set => transform.position = value;
    }

    public Quaternion Rotation
    {
      set => transform.rotation = value;
    }
  }
}