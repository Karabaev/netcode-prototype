using System.Collections.Generic;
using Motk.Client.Combat.Grid.Hex.Descriptors;
using Motk.PathFinding.Runtime;

namespace Motk.Client.Combat.Grid.Hex.Model
{
  public class HexGrid
  {
    private static readonly HexCoordinates[] Directions =
    {
      new(1, -1),
      new(1, 0),
      new(0, 1),
      new(-1, 1),
      new(-1, 0),
      new(0, -1)
    };
    
    private readonly HexGridGraph _graph = new();

    public IEnumerable<HexGridNode> Nodes => _graph.Nodes.Values;

    public void Initialize(HexMapDescriptor descriptor)
    {
      foreach (var nodeDescription in descriptor.Nodes)
      {
        var node = new HexGridNode(nodeDescription.Coordinates, new MapNodeInfo(nodeDescription.IsWalkable, 1.0f));
        _graph.AddNode(nodeDescription.Coordinates, node);
      }
      
      foreach (var node in Nodes)
      {
        if (!node.Info.IsWalkable)
          continue;

        foreach (var direction in Directions)
        {
          var neighborCoordinates = node.Coordinates + direction;
          var neighborExists = TryGetNode(neighborCoordinates, out var neighbor);
          
          if (!neighborExists || !neighbor!.Info.IsWalkable)
            continue;
          
          _graph.AddEdge(node.Coordinates, neighborCoordinates);
        }
      }
    }

    public HexGridNode RequireNode(HexCoordinates coordinates) => _graph.RequireNode(coordinates);

    public bool TryGetNode(HexCoordinates coordinates, out HexGridNode? result)
    {
      if (!_graph.TryGetNode(coordinates, out var node))
      {
        result = null;
        return false;
      }

      result = node;
      return true;
    }
  }
}