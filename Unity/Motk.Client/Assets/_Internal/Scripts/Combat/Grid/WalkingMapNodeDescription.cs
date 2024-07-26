using UnityEngine;

namespace Motk.Client.Combat.Grid
{
  public class WalkingMapNodeDescription
  {
    public readonly Vector2Int Position;
    public readonly bool IsWalkable;

    public WalkingMapNodeDescription(Vector2Int position, bool isWalkable)
    {
      Position = position;
      IsWalkable = isWalkable;
    }
  }
}