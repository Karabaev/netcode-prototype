using Motk.Client.Combat.Units;
using Motk.Client.Combat.Units.Core;
using Motk.Combat.Shared;

namespace Motk.Client.Combat
{
  public static class CombatStateExtensions
  {
    public static CombatUnitState RequireUnit(this CombatState combatState, CombatUnitIdentifier identifier)
    {
      return combatState.Teams.Require(identifier.TeamId).Units.Require(identifier.UnitId);
    }
  }
}