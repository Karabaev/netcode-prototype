using Motk.Combat.Client.Core.Units;
using Motk.Combat.Shared;
using Motk.HexGrid.Core;

namespace Motk.Combat.Client.Core
{
  public static class CombatStatesUtils
  {
    public static CombatUnitIdentifier FromDto(CombatUnitIdentifierDto dto)
    {
      return new CombatUnitIdentifier(dto.TeamId, dto.UnitId);
    }

    public static CombatTeamState FromDto(ushort teamId, CombatTeamDto dto)
    {
      var state = new CombatTeamState();
      foreach (var (unitId, unitDto) in dto.Units)
      {
        var identifier = new CombatUnitIdentifier(teamId, unitId);
        state.Units.Add(unitId, new CombatUnitState(unitDto.DescriptorId, identifier, unitDto.Count, unitDto.CurrentHp, unitDto.Position, HexDirection.NE));
      }

      return state;
    }
  }
}