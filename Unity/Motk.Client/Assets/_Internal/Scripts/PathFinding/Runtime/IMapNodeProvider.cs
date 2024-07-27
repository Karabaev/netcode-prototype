using System;
using System.Collections.Generic;

namespace Motk.PathFinding.Runtime
{
  public interface IMapNodeProvider<TCoords> where TCoords : IEquatable<TCoords>
  {
    // нужны соседние ноды: позиция, проходимость, стоимость
    IReadOnlyCollection<(TCoords, MapNodeInfo)> RequireNodeNeighborInfos(TCoords nodeCoordinates);
  }
}