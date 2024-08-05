using com.karabaev.reactivetypes.Action;

namespace Motk.Editor.CombatArenaEditor
{
  public class CombatArenaEditorModel
  {
    public ReactiveAction<string> ErrorOccured { get; } = new();

  }
}