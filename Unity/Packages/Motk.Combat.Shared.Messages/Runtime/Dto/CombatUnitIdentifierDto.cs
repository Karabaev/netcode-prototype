using MessagePack;

namespace Motk.Combat.Shared.Messages.Dto
{
  [MessagePackObject]
  public readonly struct CombatUnitIdentifierDto
  {
    [Key(0)] public readonly ushort TeamId;
    [Key(1)] public readonly ushort UnitId;

    public CombatUnitIdentifierDto(ushort teamId, ushort unitId)
    {
      TeamId = teamId;
      UnitId = unitId;
    }
  }
}