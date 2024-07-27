﻿namespace Motk.PathFinding.Runtime
{
  public readonly struct MapNodeInfo
  {
    public readonly bool IsWalkable;
    public readonly float Cost;

    public MapNodeInfo(bool isWalkable, float cost)
    {
      IsWalkable = isWalkable;
      Cost = cost;
    }
  }
}