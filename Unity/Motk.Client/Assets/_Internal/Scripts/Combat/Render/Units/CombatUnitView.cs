using com.karabaev.utilities.unity.GameKit;
using Motk.Combat.Client.Core.Units;
using UnityEngine;

namespace Motk.Combat.Client.Render.Units
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