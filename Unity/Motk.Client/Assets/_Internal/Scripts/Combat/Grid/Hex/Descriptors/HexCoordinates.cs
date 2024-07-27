using System;
using UnityEngine;

namespace Motk.Client.Combat.Grid.Hex.Descriptors
{
  /// <summary>
  /// Axial coordinates.
  /// </summary>
  public readonly struct HexCoordinates : IEquatable<HexCoordinates>
  {
    public readonly int Q;
    public readonly int R;

    public int S => -Q - R;
    
    public Vector3 ToWorld(float hexRadius, float y)
    {
      var x = hexRadius * Mathf.Sqrt(3) * (Q + R / 2.0f);
      var z = hexRadius * 1.5f * R;
      return new Vector3(x, y, z);
    }
    
    public static HexCoordinates FromWorld(Vector3 position, float hexRadius)
    {
      var q = (Mathf.Sqrt(3f) / 3f * position.x - 1f / 3f * position.z) / hexRadius;
      var r = 2.0f / 3.0f * position.z / hexRadius;

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

    public static int Distance(HexCoordinates from, HexCoordinates to)
    {
      return (Math.Abs(from.Q - to.Q) + Math.Abs(from.R - to.R) + Math.Abs(from.S - to.S)) / 2;
    }

    public override string ToString() => $"{Q}:{R}";

    public bool Equals(HexCoordinates other) => Q == other.Q && R == other.R;

    public override bool Equals(object? obj) => obj is HexCoordinates other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Q, R);

    public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b)
    {
      return new HexCoordinates(a.Q + b.Q, a.R + b.R);
    }
    
    public static HexCoordinates operator -(HexCoordinates a, HexCoordinates b)
    {
      return new HexCoordinates(a.Q - b.Q, a.R - b.R);
    }
    
    public HexCoordinates(int q, int r)
    {
      Q = q;
      R = r;
    }
  }
}