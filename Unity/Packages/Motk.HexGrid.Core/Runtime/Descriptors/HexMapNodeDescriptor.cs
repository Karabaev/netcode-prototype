using System;

namespace Motk.HexGrid.Core.Descriptors
{
  [Serializable]
  public class HexMapNodeDescriptor // todokmo заменить
  {
    public HexCoordinates Coordinates;
    public bool IsWalkable;
  }
}