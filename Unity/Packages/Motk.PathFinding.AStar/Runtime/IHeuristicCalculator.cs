using System;

namespace Motk.PathFinding.AStar
{
  public interface IHeuristicCalculator<in TCoords> where TCoords : IEquatable<TCoords>
  {
    float Calculate(TCoords from, TCoords to);
  }
}