using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Shared.Messages.Client
{
  public readonly struct MoveActionCommand
  {
    public readonly HexCoordinatesDto Destination;
  }
}