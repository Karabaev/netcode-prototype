using Motk.HexGrid.Core.Descriptors;
using UnityEngine;

namespace Mork.HexGrid.Render.Unity.Functions
{
  public class FlatToppedHexGridFunctions : IHexGridFunctions
  {
    public Vector3 GetLocalCorner(int index, float radius)
    {
      var angle = Mathf.Deg2Rad * -(60.0f * index);
      var x = radius * Mathf.Cos(angle);
      var z = radius * Mathf.Sin(angle);
      return new Vector3(x, 0.0f, z);
    }

    public Vector3 ToLocal(HexCoordinates coordinates, float radius)
    {
      var x = radius * 1.5f * coordinates.Q;
      var z = radius * Mathf.Sqrt(3.0f) * (coordinates.R + coordinates.Q / 2.0f);
      return new Vector3(x, 0.0f, z);
    }

    public HexCoordinates ToHexCoordinates(Vector3 position, float radius)
    {
      var q = 2.0f / 3.0f * position.x / radius;
      var r = (-1.0f / 3.0f * position.x + Mathf.Sqrt(3.0f) / 3.0f * position.z) / radius;

      // Round to the nearest hex
      var roundedQ = Mathf.RoundToInt(q);
      var roundedR = Mathf.RoundToInt(r);
      var roundedS = Mathf.RoundToInt(-q - r);

      // Correct rounding errors to ensure q + r + s == 0
      if (roundedQ + roundedR + roundedS != 0)
      {
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
      }

      return new HexCoordinates(roundedQ, roundedR);
    }
  }
}