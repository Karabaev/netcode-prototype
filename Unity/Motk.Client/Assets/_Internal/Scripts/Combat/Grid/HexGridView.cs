using System.Collections.Generic;
using UnityEngine;

namespace Motk.Client.Combat.Grid
{
  public class HexGridView : MonoBehaviour
  {
    [SerializeField]
    private HexGridNodeView _nodePrefab = null!;
    
    private HexGrid _grid = null!;

    private Dictionary<(int, int), HexGridNodeView> _nodes = new();

    private void Start()
    {
      _grid = new HexGrid();

      var gridDescription = new WalkingMapDescription
      {
        Nodes = new List<WalkingMapNodeDescription>
        {
          // new(new Vector2Int(-2, -2), true),
          // new(new Vector2Int(-2, -1), true),
          // new(new Vector2Int(-2, 0), true),
          // new(new Vector2Int(-2, 1), true),
          // new(new Vector2Int(-2, 2), true),
          // new(new Vector2Int(-1, -2), true),
          // new(new Vector2Int(-1, -1), true),
          // new(new Vector2Int(-1, 0), true),
          // new(new Vector2Int(-1, 1), true),
          // new(new Vector2Int(-1, 2), true),
          // new(new Vector2Int(0, -2), true),
          // new(new Vector2Int(0, -1), true),
          // new(new Vector2Int(0, 0), true),
          // new(new Vector2Int(0, 1), true),
          // new(new Vector2Int(0, 2), true),
          // new(new Vector2Int(1, -2), true),
          // new(new Vector2Int(1, -1), true),
          // new(new Vector2Int(1, 0), true),
          // new(new Vector2Int(1, 1), true),
          // new(new Vector2Int(1, 2), true),
          // new(new Vector2Int(2, -2), true),
          // new(new Vector2Int(2, -1), true),
          // new(new Vector2Int(2, 0), true),
          // new(new Vector2Int(2, 1), true),
          // new(new Vector2Int(2, 2), true)
          new(new Vector2Int(0, 0), true),
          new(new Vector2Int(0, 1), true),
          new(new Vector2Int(0, 2), true),
          new(new Vector2Int(0, 3), true),
          new(new Vector2Int(0, 4), true),
          new(new Vector2Int(0, 5), true),
          new(new Vector2Int(1, 0), true),
          new(new Vector2Int(1, 1), true),
          new(new Vector2Int(1, 2), true),
          new(new Vector2Int(1, 3), true),
          new(new Vector2Int(1, 4), true),
          new(new Vector2Int(1, 5), true),
          new(new Vector2Int(2, 0), true),
          new(new Vector2Int(2, 1), true),
          new(new Vector2Int(2, 2), true),
          new(new Vector2Int(2, 3), true),
          new(new Vector2Int(2, 4), true),
          new(new Vector2Int(2, 5), true)
        }
      };
      _grid.Initialize(gridDescription);
      
      foreach (var (position, node) in _grid.Nodes)
      {
        var nodeView = CreateNode(position.Item1, position.Item2);
        _nodes.Add(position, nodeView);
      }
    }

    private HexGridNodeView CreateNode(int x, int z)
    {
      var node = Instantiate(_nodePrefab, transform);
      var position = new Vector3(
        HexMetrics.InnerRadius * 2 * (x + z * 0.5f - z / 2), 
        0,
        HexMetrics.OuterRadius * 1.5f * z);
      node.transform.localPosition = position;
      node.Position = new Vector2Int(x, z);
      return node;
    }
  }
}