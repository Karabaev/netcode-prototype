using System.Collections.Generic;
using System.Linq;
using Motk.Client.Combat.Grid.Hex.Descriptors;
using Motk.PathFinding.Runtime;

namespace Motk.Client.Combat.Grid.Hex.Model
{
  public class HexGridMapNodeProvider : IMapNodeProvider<HexCoordinates>
  {
    private readonly HexGrid _grid;

    public IReadOnlyCollection<(HexCoordinates, MapNodeInfo)> RequireNodeNeighborInfos(HexCoordinates nodeCoordinates)
    {
      var node = _grid.RequireNode(nodeCoordinates);
      return node.Neighbors.Select(n => (n.Coordinates, n.Info)).ToList();
    }

    public HexGridMapNodeProvider(HexGrid grid) => _grid = grid;
  }
}