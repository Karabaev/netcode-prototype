using System;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Shared.Descriptors
{
  [Serializable]
  public class CombatArenaDescriptor
  {
    public HexMapNode[] Map = null!;
    public TeamStartPlacements[] TeamPlacements = null!;
  }

  [Serializable]
  public class HexMapNode
  {
    // todokmo add possible edges to prevent move to wall hexes from outside
    public HexCoordinates Coordinates;
    public bool IsWalkable;
    public bool IsHighGround;
  }

  [Serializable]
  public class TeamStartPlacements
  {
    public HexGridPlacement[] StartPlacements = null!;
  }

  [Serializable]
  public class HexGridPlacement
  {
    public HexCoordinates Position;
    public HexDirection Direction;
  }
}