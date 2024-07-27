using System.Collections.Generic;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Mork.HexGrid.Render.Unity
{
  public class HexGridView : MonoBehaviour
  {
    [SerializeField]
    private HexGridNodeView _nodePrefab = null!;
    
    private Motk.HexGrid.Core.HexGrid _grid = null!;

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

      var position = coordinates.ToWorld(0.0f);
      node.transform.localPosition = position;
      node.Position = coordinates;
      return node;
    }
  }
}