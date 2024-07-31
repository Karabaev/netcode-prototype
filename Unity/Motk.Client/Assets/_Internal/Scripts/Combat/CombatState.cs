using com.karabaev.reactivetypes.Collection;
using com.karabaev.reactivetypes.Dictionary;
using com.karabaev.reactivetypes.Property;
using JetBrains.Annotations;
using Motk.Combat.Shared;

namespace Motk.Client.Combat
{
  [UsedImplicitly]
  public class CombatState
  {
    public ReactiveProperty<ushort> RoundIndex { get; } = new(ushort.MaxValue);

    public ReactiveCollection<CombatUnitIdentifier> TurnsQueue { get; } = new();

    public ReactiveDictionary<ushort, CombatTeamState> Teams { get; } = new();
  }
}