using System;

namespace Motk.PathFinding.Runtime
{
  public interface IHeuristicCalculator<in TCoords> where TCoords : IEquatable<TCoords>
  {
    float Calculate(TCoords from, TCoords to);
  }
}