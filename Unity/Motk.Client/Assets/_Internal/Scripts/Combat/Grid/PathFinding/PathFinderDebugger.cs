using System.Collections;
using System.Collections.Generic;
using Motk.Client.Combat.Grid.Hex.Descriptors;
using Motk.Client.Combat.Grid.Hex.Model;
using Motk.Client.Combat.Grid.Hex.View;
using Motk.PathFinding.Runtime;
using UnityEngine;

namespace Motk.Client.Combat.Grid.PathFinding
{
  public class PathFinderDebugger : MonoBehaviour
  {
    private HexGrid _grid = null!;
    private HexCoordinates _currentCoords;
    private HexGridView _gridView = null!;

    private AStarPathFindingService<HexCoordinates> _pathFindingService = null!;

    [SerializeField]
    private Transform _pawn = null!;
    
    private void Start()
    {
      _grid = FindObjectOfType<HexGridProvider>().Grid;
      _gridView = FindObjectOfType<HexGridView>();

      var mapNodeProvider = new HexGridMapNodeProvider(_grid);
      var heuristicCalculator = new HexHeuristicCalculator();
      _pathFindingService = new AStarPathFindingService<HexCoordinates>(mapNodeProvider, heuristicCalculator);

      _pawn.transform.position = new HexCoordinates(0, 0).ToWorld(HexMetrics.OuterRadius, 0.0f);
    }
    
    private void Update()
    {
      if (!Input.GetMouseButtonDown(0))
        return;

      var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
      if (!Physics.Raycast(ray, out var hitInfo))
        return;

      var destination = HexCoordinates.FromWorld(hitInfo.point, HexMetrics.OuterRadius);

      var origin = HexCoordinates.FromWorld(_pawn.position, HexMetrics.OuterRadius);
      var path = _pathFindingService.FindPath(origin, destination);

      if (path.Count == 0)
        return;

      StartCoroutine(Move(path));
    }

    private IEnumerator Move(Stack<HexCoordinates> path)
    {
      while (path.TryPop(out var nextCoordinates))
      {
        yield return new WaitForSeconds(0.5f);
        _pawn.position = nextCoordinates.ToWorld(HexMetrics.OuterRadius, 0.0f);
      }
    }
  }
}