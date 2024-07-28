using System.Globalization;

namespace Motk.Combat.Server.Core.Primitives
{
  public readonly struct Vector2Int : IEquatable<Vector2Int>, IFormattable
  {
    public readonly int X;
    public readonly int Y;

    public static readonly Vector2Int Zero = new(0, 0);
    public static readonly Vector2Int One = new(1, 1);
    public static readonly Vector2Int Up = new(0, 1);
    public static readonly Vector2Int Forward = new(0, 1);

    public bool Equals(Vector2Int other)
    {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
      return obj is Vector2Int other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
      return ToString(null, null);
    }

    public string ToString(string format)
    {
      return ToString(format, null);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
      formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
      return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)})";
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);

    public static Vector2Int operator -(Vector2Int a) => new(-a.X, -a.Y);

    public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
    {
      var num1 = lhs.X - rhs.X;
      var num2 = lhs.Y - rhs.Y;
      return num1 * (double)num1 + num2 * (double)num2 < 9.999999439624929E-11;
    }

    public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
    {
      return !(lhs == rhs);
    }

    public static Vector2Int operator *(Vector2Int a, int d) => new(a.X * d, a.Y * d);
    
    public Vector2Int(int x, int y)
    {
      X = x;
      Y = y;
    }
  }
}