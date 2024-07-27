using System;
using Motk.Client.Combat.Grid.Hex.Descriptors;
using Motk.PathFinding.Runtime;

namespace Motk.Client.Combat.Grid.Hex.Model
{
  public class HexHeuristicCalculator : IHeuristicCalculator<HexCoordinates>
  {
    public float Calculate(HexCoordinates from, HexCoordinates to)
    {
      return (MathF.Abs(from.Q - to.Q) + MathF.Abs(from.R - to.R) + MathF.Abs(from.S - to.S)) / 2.0f;
    }
  }
}