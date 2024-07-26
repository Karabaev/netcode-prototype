using System;
using System.Collections.Generic;

namespace Motk.Client.Combat.Grid
{
  public interface IMutableHexGridNode
  {
    void AddNeighbor(HexGridNode node);
  }
  
  public class HexGridNode : IMutableHexGridNode, IEquatable<HexGridNode>
  {
    public readonly int Q;
    public readonly int R;
    public readonly int S;
    public readonly bool IsWalkable;

    private readonly List<HexGridNode> _neighbors = new();
    public IReadOnlyList<HexGridNode> Neighbors => _neighbors;

    void IMutableHexGridNode.AddNeighbor(HexGridNode node)
    {
      if (_neighbors.Contains(node))
        return;
      
      _neighbors.Add(node);
    }

    public HexGridNode(int q, int r, bool isWalkable)
    {
      Q = q;
      R = r;
      S = -q - r;
      IsWalkable = isWalkable;
    }

    public bool Equals(HexGridNode? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Q == other.Q && R == other.R && S == other.S;
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((HexGridNode)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Q, R, S);
  }
}