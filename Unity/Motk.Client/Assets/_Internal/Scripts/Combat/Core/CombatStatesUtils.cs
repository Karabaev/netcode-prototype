using Motk.Combat.Client.Core.Units;
using Motk.Combat.Shared;
using Motk.Combat.Shared.Messages.Dto;
using Motk.HexGrid.Core;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Client.Core
{
  public static class CombatStatesUtils
  {
    public static CombatUnitIdentifier FromDto(in CombatUnitIdentifierDto dto)
    {
      return new CombatUnitIdentifier(dto.TeamId, dto.UnitId);
    }

    public static CombatTeamState FromDto(in CombatTeamDto dto)
    {
      var state = new CombatTeamState();
      foreach (var unitDto in dto.Units)
      {
        var identifier = new CombatUnitIdentifier(dto.TeamId, unitDto.Id);
        var position = new HexCoordinates(unitDto.Position.Q, unitDto.Position.R);
        state.Units.Add(unitDto.Id, new CombatUnitState(unitDto.DescriptorId, identifier, unitDto.Count, unitDto.CurrentHp, position, HexDirection.NE));
      }

      return state;
    }
  }
}