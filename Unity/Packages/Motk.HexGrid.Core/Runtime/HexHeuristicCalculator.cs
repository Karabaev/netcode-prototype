using System;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.AStar;

namespace Motk.HexGrid.Core
{
  public class HexHeuristicCalculator : IHeuristicCalculator<HexCoordinates>
  {
    public float Calculate(HexCoordinates from, HexCoordinates to)
    {
      return (MathF.Abs(from.Q - to.Q) + MathF.Abs(from.R - to.R) + MathF.Abs(from.S - to.S)) / 2.0f;
    }
  }
}