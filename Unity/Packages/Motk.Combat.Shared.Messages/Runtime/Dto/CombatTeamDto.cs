using MessagePack;

namespace Motk.Combat.Shared.Messages.Dto
{
  [MessagePackObject]
  public readonly struct CombatTeamDto
  {
    [Key(0)] public readonly byte TeamId;
    [Key(1)] public readonly CombatUnitDto[] Units;

    public CombatTeamDto(byte teamId, CombatUnitDto[] units)
    {
      TeamId = teamId;
      Units = units;
    }
  }
}