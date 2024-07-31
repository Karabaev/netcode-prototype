using com.karabaev.reactivetypes.Dictionary;
using Motk.Client.Combat.Units;
using Motk.Client.Combat.Units.Core;

namespace Motk.Client.Combat
{
  public class CombatTeamState
  {
    public ReactiveDictionary<ushort, CombatUnitState> Units { get; } = new();
  }
}