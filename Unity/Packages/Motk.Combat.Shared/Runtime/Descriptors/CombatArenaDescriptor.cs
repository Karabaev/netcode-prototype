using System;
using System.Collections.Generic;
using com.karabaev.descriptors.abstractions;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Shared.Descriptors
{
  [Serializable]
  public class CombatArenaDescriptor : IDescriptor
  {
    public IReadOnlyList<HexMapNode> Map = null!;
    public IReadOnlyList<TeamStartPlacements> TeamPlacements = null!;
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
    public IReadOnlyList<HexGridPlacement> StartPlacements = null!;
  }

  [Serializable]
  public class HexGridPlacement
  {
    public HexCoordinates Position;
    public HexDirection Direction;
  }
}