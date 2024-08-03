using com.karabaev.reactivetypes.Dictionary;
using Motk.Combat.Client.Core.Units;

namespace Motk.Combat.Client.Core
{
  public class CombatTeamState
  {
    public ReactiveDictionary<ushort, CombatUnitState> Units { get; } = new();
  }
}