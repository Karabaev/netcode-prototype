using System;

namespace Motk.PathFinding.AStar
{
  public readonly struct AStarNode<TCoords> : IEquatable<AStarNode<TCoords>>, IComparable<AStarNode<TCoords>>
    where TCoords : struct, IEquatable<TCoords>
  {
    public readonly TCoords Coordinates;
    /// <summary>
    /// Distance from start of path to current node (G).
    /// </summary>
    public readonly float TraversedDistance;
    /// <summary>
    /// Estimated movement cost from start to destination (F). 
    /// </summary>
    public readonly float EstimatedTotalCost;

    public int CompareTo(AStarNode<TCoords> other) => EstimatedTotalCost.CompareTo(other.EstimatedTotalCost);

    public bool Equals(AStarNode<TCoords> other)
    {
      return Coordinates.Equals(other.Coordinates)
             && TraversedDistance.Equals(other.TraversedDistance)
             && EstimatedTotalCost.Equals(other.EstimatedTotalCost);
    }

    public override bool Equals(object? obj) => obj is AStarNode<TCoords> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Coordinates, TraversedDistance, EstimatedTotalCost);

    public AStarNode(TCoords coordinates, float traversedDistance, float estimatedTotalCost)
    {
      Coordinates = coordinates;
      TraversedDistance = traversedDistance;
      EstimatedTotalCost = estimatedTotalCost;
    }
  }
}