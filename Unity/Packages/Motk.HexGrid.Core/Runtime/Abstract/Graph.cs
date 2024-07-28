using System;
using System.Collections.Generic;

namespace Motk.HexGrid.Core.Abstract
{
  public abstract class Graph<TId, TNode> 
    where TId : IEquatable<TId>
    where TNode : IGraphNode
  {
    private readonly Dictionary<TId, TNode> _nodes = new();

    public IReadOnlyDictionary<TId, TNode> Nodes => _nodes;

    public TNode RequireNode(TId id) => _nodes[id];

    public bool TryGetNode(TId coordinates, out TNode? result) => _nodes.TryGetValue(coordinates, out result);

    public void AddNode(TId id, TNode node) => _nodes.Add(id, node);
    
    public void RemoveNode(TId id)
    {
      _nodes.Remove(id, out var removedNode);

      foreach (var neighbor in removedNode.Neighbors)
      {
        neighbor.RemoveNeighbor(removedNode);
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