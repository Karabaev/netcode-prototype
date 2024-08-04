using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Shared.Messages.Client
{
  public readonly struct SpellBookActionCommand
  {
    public readonly ushort SpellId;
    public readonly HexCoordinatesDto Target;
  }
}