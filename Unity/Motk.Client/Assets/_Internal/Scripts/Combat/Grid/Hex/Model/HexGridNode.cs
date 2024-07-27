using System;
using System.Collections.Generic;
using Motk.Client.Combat.Grid.Abstract;
using Motk.Client.Combat.Grid.Hex.Descriptors;
using Motk.PathFinding.Runtime;

namespace Motk.Client.Combat.Grid.Hex.Model
{
  public class HexGridNode : IEquatable<HexGridNode>, IGraphNode
  {
    public readonly HexCoordinates Coordinates;
    public readonly MapNodeInfo Info;
    
    private readonly HashSet<HexGridNode> _neighbors = new();
    public IReadOnlyCollection<HexGridNode> Neighbors => _neighbors;

    IReadOnlyCollection<IGraphNode> IGraphNode.Neighbors => _neighbors;

    void IMutableGraphNode.AddNeighbor(IMutableGraphNode mutableNode)
    {
      var node = (HexGridNode) mutableNode;
      if (_neighbors.Contains(node))
        return;
      
      _neighbors.Add(node);
    }

    void IMutableGraphNode.RemoveNeighbor(IMutableGraphNode mutableNode)
    {
      var node = (HexGridNode) mutableNode;
      _neighbors.Remove(node);
    }

    public bool Equals(HexGridNode? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Coordinates.Equals(other.Coordinates);
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((HexGridNode)obj);
    }

    public override int GetHashCode() => Coordinates.GetHashCode();

    public HexGridNode(HexCoordinates coordinates, MapNodeInfo info)
    {
      Coordinates = coordinates;
      Info = info;
    }
  }
}