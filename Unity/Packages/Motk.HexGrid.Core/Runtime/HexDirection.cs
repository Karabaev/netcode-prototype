namespace Motk.HexGrid.Core
{
  public enum HexDirection : ushort
  {
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
      return (ushort) source < 3 ? source + 3 : source - 3;
    }
  }
}