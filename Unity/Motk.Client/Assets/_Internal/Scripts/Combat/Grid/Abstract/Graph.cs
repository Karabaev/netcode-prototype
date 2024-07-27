using System;
using System.Collections.Generic;

namespace Motk.Client.Combat.Grid.Abstract
{
  public class Graph<TId, TPayload> where TId : IEquatable<TId>
  {
    private readonly Dictionary<TId, GraphNode<TId, TPayload>> _nodes = new();

    public IReadOnlyDictionary<TId, GraphNode<TId, TPayload>> Nodes => _nodes;

    public GraphNode<TId, TPayload> RequireNode(TId id) => _nodes[id];

    public bool TryGetNode(TId coordinates, out GraphNode<TId, TPayload>? result)
    {
      return _nodes.TryGetValue(coordinates, out result);
    }

    public void AddNode(TId id, TPayload payload)
    {
      _nodes.Add(id, new GraphNode<TId, TPayload>(id, payload));
    }
    
    public void AddNode(GraphNode<TId, TPayload> node)
    {
      _nodes.Add(node.Id, node);
    }

    public void RemoveNode(TId id)
    {
      _nodes.Remove(id, out var removedNode);

      foreach (var neighbor in removedNode.Neighbors)
      {
        ((IMutableGraphNode) neighbor).RemoveNeighbor(removedNode);
      }
    }
    
    public void AddEdge(TId a, TId b)
    {
      IMutableGraphNode nodeA = RequireNode(a);
      IMutableGraphNode nodeB = RequireNode(b);
      
      nodeA.AddNeighbor(nodeB);
      nodeB.AddNeighbor(nodeA);
    }
  }
}