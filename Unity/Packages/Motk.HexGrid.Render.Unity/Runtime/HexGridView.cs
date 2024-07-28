using System.Collections.Generic;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Mork.HexGrid.Render.Unity
{
  public class HexGridView : MonoBehaviour
  {
    [SerializeField]
    private HexGridNodeView _nodePrefab = null!;
    
    private Motk.HexGrid.Core.HexGrid _grid = null!;

    private HexGridState _state = null!;
    private readonly Dictionary<HexCoordinates, HexGridNodeView> _nodes = new();

    public void Construct(HexGridState state, Motk.HexGrid.Core.HexGrid grid)
    {
      _state = state;
      _grid = grid;
      
      foreach (var node in _grid.Nodes)
      {
        var nodeView = CreateNodeView(node);
        _nodes.Add(node.Coordinates, nodeView);
      }
    }
    
    private HexGridNodeView CreateNodeView(HexGridNode node)
    {
      var nodeView = Instantiate(_nodePrefab, transform);
      var nodeState = new HexGridNodeState(node.Coordinates, node.Info.IsWalkable);
      var position = node.Coordinates.ToWorld(0.0f);
      nodeView.transform.localPosition = position;
      nodeView.Construct(nodeState);
      return nodeView;
    }
  }
}