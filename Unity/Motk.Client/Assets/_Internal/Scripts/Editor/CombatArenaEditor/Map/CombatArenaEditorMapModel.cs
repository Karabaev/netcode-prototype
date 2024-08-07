using com.karabaev.reactivetypes.Collection;

namespace Motk.Editor.CombatArenaEditor.Map
{
  public class CombatArenaEditorMapModel
  {
    public ReactiveCollection<HexGridNode> Nodes { get; } = new();
  }
}