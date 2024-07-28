using System;

namespace Motk.HexGrid.Core.Descriptors
{
  [Serializable]
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