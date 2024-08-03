using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Client.Core.Network.Client
{
  public readonly struct SpellBookActionCommand
  {
    public readonly ushort SpellId;
    public readonly HexCoordinates Target;
  }
}