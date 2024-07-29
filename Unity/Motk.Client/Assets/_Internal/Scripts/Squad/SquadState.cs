using com.karabaev.reactivetypes.Collection;
using com.karabaev.reactivetypes.Property;

namespace Motk.Client.Squad
{
  public class SquadState
  {
    public ReactiveCollection<SquadUnitState> Units { get; } = new();
  }

  public class SquadUnitState
  {
    public string UnitId { get; }
    
    public ReactiveProperty<int> Count { get; } = new(0);

    public SquadUnitState(string unitId) => UnitId = unitId;
  }
}