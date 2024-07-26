namespace Motk.Client.Combat.Grid
{
  public readonly struct HexCoordinates
  {
    public readonly int X;
    public readonly int Z;

    public HexCoordinates(int x, int z)
    {
      X = x;
      Z = z;
    }
  }
}