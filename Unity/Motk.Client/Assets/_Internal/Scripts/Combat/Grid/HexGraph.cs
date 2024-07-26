using System.Collections.Generic;
using UnityEngine;

namespace Motk.Client.Combat.Grid
{
  public class HexGraph
  {
    private readonly Dictionary<(int, int), HexGridNode> _nodes = new();

    public IReadOnlyDictionary<(int, int), HexGridNode> Nodes => _nodes;

    public HexGridNode RequireNode(int q, int r) => _nodes[(q, r)];

    public bool TryGetNode(int q, int r, out HexGridNode result) => _nodes.TryGetValue((q, r), out result);

    public void AddNode(int q, int r, bool isWalkable) => _nodes.Add((q, r), new HexGridNode(q, r, isWalkable));

    public void AddEdge(int q1, int r1, int q2, int r2)
    {
      IMutableHexGridNode node1 = RequireNode(q1, r1);
      IMutableHexGridNode node2 = RequireNode(q2, r2);
      
      node1.AddNeighbor((HexGridNode) node2);
      node2.AddNeighbor((HexGridNode) node1);
    }
    
    public int HexDistance(HexGridNode a, HexGridNode b)
    {
      return (Mathf.Abs(a.Q - b.Q) + Mathf.Abs(a.R - b.R) + Mathf.Abs(a.S - b.S)) / 2;
    } 
  }
}