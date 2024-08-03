using Motk.Combat.Client.Core.Units;
using Motk.Combat.Shared;

namespace Motk.Combat.Client.Core
{
  public static class CombatStateExtensions
  {
    public static CombatUnitState RequireUnit(this CombatState combatState, CombatUnitIdentifier identifier)
    {
      return combatState.Teams.Require(identifier.TeamId).Units.Require(identifier.UnitId);
    }
  }
}