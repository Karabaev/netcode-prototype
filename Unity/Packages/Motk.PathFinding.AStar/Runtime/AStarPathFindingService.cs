using System;
using System.Collections.Generic;
using Motk.Collections;

namespace Motk.PathFinding.AStar
{
  public class AStarPathFindingService<TCoords> where TCoords : struct, IEquatable<TCoords>
  {
    private readonly IMapNodeProvider<TCoords> _mapNodeProvider;
    private readonly IHeuristicCalculator<TCoords> _heuristicCalculator;

    public Stack<TCoords> FindPath(TCoords origin, TCoords destination)
    {
      var totalCost = _heuristicCalculator.Calculate(origin, destination);
      var originNode = new AStarNode<TCoords>(origin, 0, totalCost);
      var opened = new BinaryHeap<TCoords, AStarNode<TCoords>>(n => n.Coordinates);
      var closed = new HashSet<TCoords>();
      var nodeLinks = new Dictionary<TCoords, TCoords>();
      
      opened.Enqueue(originNode);

      while (opened.Count > 0)
      {
        var currentNode = opened.Dequeue();
        closed.Add(currentNode.Coordinates);

        if (currentNode.Coordinates.Equals(destination))
          break;

        var nodeNeighborInfos = _mapNodeProvider.RequireNodeNeighborInfos(currentNode.Coordinates);
        foreach (var (neighborCoordinates, neighborInfo) in nodeNeighborInfos)
        {
          if (!neighborInfo.IsWalkable)
            continue;

          if (closed.Contains(neighborCoordinates))
            continue;

          var traversedDistance = currentNode.TraversedDistance + neighborInfo.Cost;
          var estimatedTotalCost = traversedDistance + _heuristicCalculator.Calculate(neighborCoordinates, destination);
          var neighborNode = new AStarNode<TCoords>(neighborCoordinates, traversedDistance, estimatedTotalCost);

          if (!opened.TryGet(neighborNode.Coordinates, out var openedNode))
          {
            opened.Enqueue(neighborNode);
            nodeLinks[neighborNode.Coordinates] = currentNode.Coordinates;
            continue;
          }

          if (neighborNode.TraversedDistance >= openedNode!.Value.TraversedDistance)
            continue;
          
          opened.Modify(neighborNode);
          nodeLinks[neighborNode.Coordinates] = currentNode.Coordinates;
        }
      }

      return BuildPath(origin, destination, nodeLinks);
    }

    private Stack<TCoords> BuildPath(TCoords origin, TCoords destination, Dictionary<TCoords, TCoords> links)
    {
      var path = new Stack<TCoords>();

      if(!links.ContainsKey(destination))
        return path;

      var current = destination;

      while(!current.Equals(origin))
      {
        path.Push(current);
        current = links[current];
      }

      return path;
    }

    public AStarPathFindingService(IMapNodeProvider<TCoords> mapNodeProvider, IHeuristicCalculator<TCoords> heuristicCalculator)
    {
      _mapNodeProvider = mapNodeProvider;
      _heuristicCalculator = heuristicCalculator;
    }
  }
}