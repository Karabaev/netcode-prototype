using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Motk.Shared.Campaign.PathFinding
{
  [UsedImplicitly]
  public class NavMeshPathFindingService
  {
    public Vector3[] FindPath(Vector3 source, Vector3 destination)
    {
      var path = new NavMeshPath();
      NavMesh.CalculatePath(source, destination, NavMesh.AllAreas, path);
      return path.corners;
    }
  }
}