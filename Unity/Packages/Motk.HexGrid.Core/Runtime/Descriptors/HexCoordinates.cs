using System;

namespace Motk.HexGrid.Core.Descriptors
{
  /// <summary>
  /// Axial coordinates.
  /// </summary>
  public readonly struct HexCoordinates : IEquatable<HexCoordinates>
  {
    public readonly int Q;
    public readonly int R;

    public int S => -Q - R;
    
    public override string ToString() => $"{Q}:{R}";

    public bool Equals(HexCoordinates other) => Q == other.Q && R == other.R;

    public override bool Equals(object? obj) => obj is HexCoordinates other && Equals(other);

    // todokmo check if HashCode.Combine returns difference value with swapped Q and R
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