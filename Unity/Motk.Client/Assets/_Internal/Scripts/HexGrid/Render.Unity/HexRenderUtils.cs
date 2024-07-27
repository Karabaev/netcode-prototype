using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Mork.HexGrid.Render.Unity
{
  public static class HexRenderUtils
  {
    private const float OuterRadius = 0.5f;
    private const float InnerRadius = OuterRadius * 0.866025404f;

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
    
    public static Vector3 ToWorld(this HexCoordinates coordinates, float y)
    {
      var x = OuterRadius * Mathf.Sqrt(3) * (coordinates.Q + coordinates.R / 2.0f);
      var z = OuterRadius * 1.5f * coordinates.R;
      return new Vector3(x, y, z);
    }

    public static HexCoordinates ToAxialCoordinates(this Vector3 position)
    {
      var q = (Mathf.Sqrt(3f) / 3f * position.x - 1f / 3f * position.z) / OuterRadius;
      var r = 2.0f / 3.0f * position.z / OuterRadius;

      var roundedQ = Mathf.RoundToInt(q);
      var roundedR = Mathf.RoundToInt(r);
      var roundedS = Mathf.RoundToInt(-q - r);

      if (roundedQ + roundedR + roundedS == 0)
        return new HexCoordinates(roundedQ, roundedR);

      var qDiff = Mathf.Abs(roundedQ - q);
      var rDiff = Mathf.Abs(roundedR - r);
      var sDiff = Mathf.Abs(roundedS + q + r);

      if (qDiff > rDiff && qDiff > sDiff)
      {
        roundedQ = -roundedR - roundedS;
      }
      else if (rDiff > sDiff)
      {
        roundedR = -roundedQ - roundedS;
      }

      return new HexCoordinates(roundedQ, roundedR);
    }
    
    public static HexCoordinates FromOffsetCoordinates(int x, int z) => new(x - z / 2, z);
  }
}