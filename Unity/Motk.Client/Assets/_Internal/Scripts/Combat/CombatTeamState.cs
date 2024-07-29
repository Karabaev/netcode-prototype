using com.karabaev.reactivetypes.Dictionary;
using Motk.Client.Combat.Units;

namespace Motk.Client.Combat
{
  public class CombatTeamState
  {
    public ReactiveDictionary<ushort, CombatUnitState> Units { get; } = new();
  }
}