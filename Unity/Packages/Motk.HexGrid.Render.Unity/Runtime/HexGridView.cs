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

    private HexGridVisualState _visualState = null!;
    private readonly Dictionary<HexCoordinates, HexGridNodeState> _nodeStates = new();
    private readonly Dictionary<HexCoordinates, HexGridNodeView> _nodeViews = new();

    public void Construct(HexGridVisualState visualState, Motk.HexGrid.Core.HexGrid grid)
    {
      _visualState = visualState;
      _grid = grid;
      
      foreach (var node in _grid.Nodes)
      {
        var nodeState = new HexGridNodeState(node.Coordinates, node.Info.IsWalkable);
        var nodeView = CreateNodeView(node, nodeState);
        _nodeStates.Add(node.Coordinates, nodeState);
        _nodeViews.Add(node.Coordinates, nodeView);
      }
      
      _visualState.VisibleNodes.ItemAdded += State_OnVisibleNodeAdded;
      _visualState.VisibleNodes.ItemRemoved += State_OnVisibleNodeRemoved;
      _visualState.VisibleNodes.Cleaned += State_OnVisibleNodesCleaned;
    }

    private void OnDestroy()
    {
      if (_visualState == null!)
        return;
      
      _visualState.VisibleNodes.ItemAdded -= State_OnVisibleNodeAdded;
      _visualState.VisibleNodes.ItemRemoved -= State_OnVisibleNodeRemoved;
      _visualState.VisibleNodes.Cleaned -= State_OnVisibleNodesCleaned;
    }

    private void State_OnVisibleNodeAdded(HexCoordinates coordinates, GridNodeVisualStateType visualStateType)
    {
      _nodeStates[coordinates].OutlineVisibility.Value = visualStateType;
    }

    private void State_OnVisibleNodeRemoved(HexCoordinates key, GridNodeVisualStateType oldValue)
    {
      _nodeStates[key].OutlineVisibility.Value = GridNodeVisualStateType.None;
    }

    private void State_OnVisibleNodesCleaned()
    {
      foreach (var (_, nodeState) in _nodeStates)
        nodeState.OutlineVisibility.Value = GridNodeVisualStateType.None;
    }

    private HexGridNodeView CreateNodeView(HexGridNode node, HexGridNodeState nodeState)
    {
      var nodeView = Instantiate(_nodePrefab, transform);
      var position = node.Coordinates.ToWorld(0.0f);
      nodeView.transform.localPosition = position;
      nodeView.Construct(nodeState);
      return nodeView;
    }
  }
}