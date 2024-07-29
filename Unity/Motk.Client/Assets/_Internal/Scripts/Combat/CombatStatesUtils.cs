using Motk.Client.Combat.Units;
using Motk.Combat.Shared;

namespace Motk.Client.Combat
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
        state.Units.Add(unitId, new CombatUnitState(unitDto.DescriptorId, identifier, unitDto.Count, unitDto.CurrentHp, unitDto.Position));
      }

      return state;
    }
  }
}