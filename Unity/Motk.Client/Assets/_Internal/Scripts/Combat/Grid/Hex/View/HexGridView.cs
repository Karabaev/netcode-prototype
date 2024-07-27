using System.Collections.Generic;
using Motk.Client.Combat.Grid.Hex.Descriptors;
using Motk.Client.Combat.Grid.Hex.Model;
using UnityEngine;

namespace Motk.Client.Combat.Grid.Hex.View
{
  public class HexGridView : MonoBehaviour
  {
    [SerializeField]
    private HexGridNodeView _nodePrefab = null!;
    
    private HexGrid _grid = null!;

    private readonly Dictionary<HexCoordinates, HexGridNodeView> _nodes = new();
    
    private void Start()
    {
      _grid = FindObjectOfType<HexGridProvider>().Grid;

      foreach (var node in _grid.Nodes)
      {
        var nodeView = CreateNode(node.Coordinates);
        _nodes.Add(node.Coordinates, nodeView);
      }
    }

    private HexGridNodeView CreateNode(HexCoordinates coordinates)
    {
      var node = Instantiate(_nodePrefab, transform);

      var position = coordinates.ToWorld(HexMetrics.OuterRadius, 0.0f);
      node.transform.localPosition = position;
      node.Position = coordinates;
      return node;
    }
  }
}