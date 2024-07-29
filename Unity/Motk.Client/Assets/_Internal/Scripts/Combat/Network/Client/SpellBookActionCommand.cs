using Motk.HexGrid.Core.Descriptors;

namespace Motk.Client.Combat.Network.Client
{
  public readonly struct SpellBookActionCommand
  {
    public readonly ushort SpellId;
    public readonly HexCoordinates Target;
  }
}