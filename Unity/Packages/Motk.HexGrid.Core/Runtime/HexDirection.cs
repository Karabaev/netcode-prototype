namespace Motk.HexGrid.Core
{
  public enum HexDirection : byte
  {
    // N = 0,
    // NE = 1,
    // SE = 2,
    // S = 3,
    // SW = 4,
    // NW = 5
    NE = 0, // 30
    E = 1, // 90
    SE = 2, // 150
    SW = 3, // 210
    W = 4, // 270
    NW = 5 // 330
  }

  public static class HexDirectionExtensions
  {
    public static HexDirection Opposite(this HexDirection source)
    {
      return (byte) source < 3 ? source + 3 : source - 3;
    }
  }
}