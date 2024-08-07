using System.Collections.Generic;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Mork.HexGrid.Render.Unity
{
  public static class HexRenderUtils
  {
    public const float OuterRadius = 0.5f;
    private const float InnerRadius = OuterRadius * 0.866025404f; // OuterRadius * Mathf.Sqrt(3.0f) * 0.5f;

    public static readonly Vector3[] Corners =
    {
      new(0f, 0f, OuterRadius),
      new(InnerRadius, 0f, 0.5f * OuterRadius),
      new(InnerRadius, 0f, -0.5f * OuterRadius),
      new(0f, 0f, -OuterRadius),
      new(-InnerRadius, 0f, -0.5f * OuterRadius),
      new(-InnerRadius, 0f, 0.5f * OuterRadius),
      new(0f, 0f, OuterRadius)
    };
    
    public static HexCoordinates FromOffsetCoordinates(int x, int z) => new(x - z / 2, z);

    public static Quaternion ToWorld(this HexDirection direction)
    {
      var angle = (float)direction * 60.0f + 30.0f;
      return Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public static HexDirection ToHex(this Quaternion quaternion)
    {
      var yAngle = quaternion.eulerAngles.y;
      var directionIndex = Mathf.RoundToInt((yAngle + -30) / 60f) % 6;
      return (HexDirection)directionIndex;
    }

    private static readonly Dictionary<HexCoordinates, HexDirection> Directions = new()
    {
      { new HexCoordinates(0, 1), HexDirection.NE },
      { new HexCoordinates(1, 0), HexDirection.E },
      { new HexCoordinates(1, -1), HexDirection.SE },
      { new HexCoordinates(0, -1), HexDirection.SW },
      { new HexCoordinates(-1, 0), HexDirection.W },
      { new HexCoordinates(-1, 1), HexDirection.NW }
    };
    
    public static HexDirection GetDirection(HexCoordinates from, HexCoordinates to)
    {
      var deltaQ = to.Q - from.Q;
      var deltaR = to.R - from.R;

      return Directions[new HexCoordinates(deltaQ, deltaR)];
    }
  }
}