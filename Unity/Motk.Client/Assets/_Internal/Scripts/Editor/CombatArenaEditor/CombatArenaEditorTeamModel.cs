using com.karabaev.reactivetypes.Collection;
using com.karabaev.reactivetypes.Property;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Editor.CombatArenaEditor
{
  public class CombatArenaEditorTeamModel
  {
    public ReactiveProperty<bool> IsActive { get; } = new(false);

    public ReactiveProperty<bool> IsBoss { get; } = new(false);

    public ReactiveCollection<HexCoordinates> UnitPositions { get; } = new();
  }
}