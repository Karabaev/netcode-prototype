using com.karabaev.reactivetypes.Dictionary;
using Motk.Combat.Shared;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Client.Core.Grid
{
  public class CombatGridState
  {
    public ReactiveDictionary<HexCoordinates, CombatUnitIdentifier> Units { get; } = new();
  }
}