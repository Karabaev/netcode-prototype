using System;
using System.Collections.Generic;

namespace Motk.PathFinding.Runtime
{
  public interface IMapNodeProvider<TCoords> where TCoords : IEquatable<TCoords>
  {
    IReadOnlyCollection<(TCoords, MapNodeInfo)> RequireNodeNeighborInfos(TCoords nodeCoordinates);
  }
}