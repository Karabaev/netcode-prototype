using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Shared.Messages.Client
{
  public readonly struct UnitSkillActionCommand
  {
    public readonly ushort SkillId;
    public readonly HexCoordinatesDto Target;
  }
}