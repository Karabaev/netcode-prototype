using System;
using System.Collections.Generic;

namespace Motk.Client.Combat.Grid.Abstract
{
  public interface IMutableGraphNode
  {
    void AddNeighbor(IMutableGraphNode node);

    void RemoveNeighbor(IMutableGraphNode node);
  }
  
  public class GraphNode<TId, TPayload> : IEquatable<GraphNode<TId, TPayload>>, IMutableGraphNode 
    where TId : IEquatable<TId>
  {
    public readonly TId Id;
    public readonly TPayload Payload;

    private readonly HashSet<GraphNode<TId, TPayload>> _neighbors = new();
    public IReadOnlyCollection<GraphNode<TId, TPayload>> Neighbors => _neighbors;

    void IMutableGraphNode.AddNeighbor(IMutableGraphNode mutableNode)
    {
      var node = (GraphNode<TId, TPayload>) mutableNode;
      if (_neighbors.Contains(node))
        return;
      
      _neighbors.Add(node);
    }

    void IMutableGraphNode.RemoveNeighbor(IMutableGraphNode mutableNode)
    {
      var node = (GraphNode<TId, TPayload>) mutableNode;
      _neighbors.Remove(node);
    }

    public GraphNode(TId id, TPayload payload)
    {
      Id = id;
      Payload = payload;
    }

    public bool Equals(GraphNode<TId, TPayload>? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      
      return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      
      return Equals((GraphNode<TId, TPayload>)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();
  }
}