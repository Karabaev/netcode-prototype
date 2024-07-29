using System.Collections.Generic;

namespace Motk.Combat.Shared
{
  public readonly struct CombatTeamDto
  {
    public readonly Dictionary<ushort, CombatUnitDto> Units;

    public CombatTeamDto(Dictionary<ushort, CombatUnitDto> units)
    {
      Units = units;
    }
  }
}