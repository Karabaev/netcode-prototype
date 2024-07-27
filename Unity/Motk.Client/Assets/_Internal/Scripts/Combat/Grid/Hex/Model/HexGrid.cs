using System.Collections.Generic;
using Motk.Client.Combat.Grid.Hex.Descriptors;

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
      new(0, -1),
    };
    
    private readonly HexGridGraph _graph = new();

    public IEnumerable<HexGridNode> Nodes
    {
      get
      {
        foreach (var (_, node) in _graph.Nodes)
        {
          yield return (HexGridNode) node;
        }
      }
    }
    
    public void Initialize(HexMapDescriptor descriptor)
    {
      foreach (var nodeDescription in descriptor.Nodes)
      {
        var node = new HexGridNode(nodeDescription.Coordinates, new CombatGridPayload(nodeDescription.IsWalkable));
        _graph.AddNode(node);
      }
      
      foreach (var node in Nodes)
      {
        if (!node.IsWalkable)
          continue;

        foreach (var direction in Directions)
        {
          var neighborCoordinates = node.Coordinates + direction;
          var neighborExists = TryGetNode(neighborCoordinates, out var neighbor);
          
          if (!neighborExists || !neighbor!.IsWalkable)
            continue;
          
          _graph.AddEdge(node.Coordinates, neighborCoordinates);
        }
      }
    }

    public bool TryGetNode(HexCoordinates coordinates, out HexGridNode? result)
    {
      if (!_graph.TryGetNode(coordinates, out var node))
      {
        result = null;
        return false;
      }

      result = (HexGridNode?) node;
      return true;
    }
  }
}