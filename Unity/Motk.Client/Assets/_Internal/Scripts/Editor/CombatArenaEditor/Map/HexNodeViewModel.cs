using UnityEngine;

namespace Motk.Editor.CombatArenaEditor.Map
{
  public class HexNodeViewModel
  {
    public Vector3 WorldPosition;
    public float Radius;
    public readonly bool IsPassable;

    public HexNodeViewModel(Vector3 worldPosition, bool isPassable, float radius)
    {
      WorldPosition = worldPosition;
      IsPassable = isPassable;
      Radius = radius;
    }
  }
}