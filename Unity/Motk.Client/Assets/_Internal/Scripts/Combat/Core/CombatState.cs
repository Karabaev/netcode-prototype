using com.karabaev.reactivetypes.Collection;
using com.karabaev.reactivetypes.Dictionary;
using com.karabaev.reactivetypes.Property;
using JetBrains.Annotations;
using Motk.Combat.Shared;

namespace Motk.Combat.Client.Core
{
  [UsedImplicitly]
  public class CombatState
  {
    public ReactiveProperty<ushort> RoundIndex { get; } = new(ushort.MaxValue);

    public ReactiveCollection<CombatUnitIdentifier> FirstPhaseTurnsQueue { get; } = new();
    
    public ReactiveCollection<CombatUnitIdentifier> SecondPhaseTurnsQueue { get; } = new();

    public ReactiveDictionary<ushort, CombatTeamState> Teams { get; } = new();
  }
}