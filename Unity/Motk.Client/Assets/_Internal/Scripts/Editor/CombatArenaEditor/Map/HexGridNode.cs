using Motk.HexGrid.Core.Descriptors;

namespace Motk.Editor.CombatArenaEditor.Map
{
  public class HexGridNode
  {
    public readonly HexCoordinates Coordinates;
    public readonly bool IsWalkable;

    public HexGridNode(HexCoordinates coordinates, bool isWalkable)
    {
      Coordinates = coordinates;
      IsWalkable = isWalkable;
    }
  }
}