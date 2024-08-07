using com.karabaev.reactivetypes.Action;
using com.karabaev.reactivetypes.Property;

namespace Motk.Editor.CombatArenaEditor.Window
{
  public class CombatArenaEditorWindowModel
  {
    public ReactiveProperty<CombatArenaEditorMode> Mode { get; }
    
    public ReactiveProperty<CombatArenaEditorTeamsMode> TeamsMode { get; }
    
    public ReactiveAction SaveButtonClicked { get; } = new();
    
    public ReactiveAction LoadButtonClicked { get; } = new();

    public ReactiveProperty<string> Message { get; } = new(string.Empty);

    public CombatArenaEditorWindowModel(CombatArenaEditorMode mode, CombatArenaEditorTeamsMode teamsMode)
    {
      Mode = new ReactiveProperty<CombatArenaEditorMode>(mode);
      TeamsMode = new ReactiveProperty<CombatArenaEditorTeamsMode>(teamsMode);
    }
  }
}