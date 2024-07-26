using System.Collections.Generic;

namespace Motk.Client.Combat.Grid
{
  public class HexGrid
  {
    private readonly HexGraph _graph = new();

    public IReadOnlyDictionary<(int, int), HexGridNode> Nodes => _graph.Nodes;
    
    public void Initialize(WalkingMapDescription description)
    {
      for (var i = 0; i < description.Nodes.Count; i++)
      {
        var nodeDescription = description.Nodes[i];
        _graph.AddNode(nodeDescription.Position.x, nodeDescription.Position.y, nodeDescription.IsWalkable);
      }

      (int q, int r, int s)[] directions = {
        (1, -1, 0), (1, 0, -1), (0, 1, -1),
        (-1, 1, 0), (-1, 0, 1), (0, -1, 1)
      };
      
      foreach (var (position, node) in _graph.Nodes)
      {
        if (!node.IsWalkable)
          continue;

        foreach (var (q, r, s) in directions)
        {
          var neighborQ = position.Item1 + q;
          var neighborR = position.Item2 + r;
          var neighborExists = _graph.TryGetNode(neighborQ, neighborR, out var neighbor);
          
          if (!neighborExists || !neighbor.IsWalkable)
            continue;
          
          _graph.AddEdge(position.Item1, position.Item2, neighborQ, neighborR);
        }
      }
    }
  }
}