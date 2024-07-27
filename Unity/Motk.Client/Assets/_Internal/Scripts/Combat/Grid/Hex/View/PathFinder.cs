using Motk.Client.Combat.Grid.Hex.Descriptors;
using Motk.Client.Combat.Grid.Hex.Model;
using UnityEngine;

namespace Motk.Client.Combat.Grid.Hex.View
{
  public class PathFinder : MonoBehaviour
  {
    private HexGrid _grid = null!;
    private HexCoordinates _currentCoords;
    private HexGridView _gridView = null!;

    private void Awake()
    {
      _grid = FindObjectOfType<HexGridProvider>().Grid;
      _gridView = FindObjectOfType<HexGridView>();
    }
    
    private void Update()
    {
      if (!Input.GetMouseButtonDown(0))
        return;

      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (!Physics.Raycast(ray, out var hitInfo))
        return;

      var hexCoordinates = HexCoordinates.FromWorld(hitInfo.point, HexMetrics.OuterRadius);
      Debug.Log($"Clicked {hexCoordinates}");
    }
  }
}