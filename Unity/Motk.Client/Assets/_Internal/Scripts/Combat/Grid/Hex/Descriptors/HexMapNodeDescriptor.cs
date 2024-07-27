using Motk.Client.Combat.Grid.Hex.Model;

namespace Motk.Client.Combat.Grid.Hex.Descriptors
{
  public class HexMapNodeDescriptor
  {
    public readonly HexCoordinates Coordinates;
    public readonly bool IsWalkable;

    public HexMapNodeDescriptor(HexCoordinates coordinates, bool isWalkable)
    {
      Coordinates = coordinates;
      IsWalkable = isWalkable;
    }
  }
}