using System.Collections;
using System.Collections.Generic;
using Mork.HexGrid.Render.Unity;
using Motk.HexGrid;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using Motk.PathFinding.Runtime;
using UnityEngine;

namespace Motk.Client.Combat.Grid.PathFinding
{
  public class PathFinderDebugger : MonoBehaviour
  {
    private HexGrid.Core.HexGrid _grid = null!;
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

      _pawn.transform.position = new HexCoordinates(0, 0).ToWorld(0.0f);
    }
    
    private void Update()
    {
      if (!Input.GetMouseButtonDown(0))
        return;

      var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
      if (!Physics.Raycast(ray, out var hitInfo))
        return;

      var destination = HexRenderUtils.FromWorld(hitInfo.point);

      var origin = HexRenderUtils.FromWorld(_pawn.position);
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
        _pawn.position = nextCoordinates.ToWorld(0.0f);
      }
    }
  }
}