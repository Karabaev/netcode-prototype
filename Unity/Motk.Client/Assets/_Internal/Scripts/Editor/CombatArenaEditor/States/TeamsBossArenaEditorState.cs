using Motk.Editor.CombatArenaEditor.Map;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Editor.CombatArenaEditor.States
{
  public class TeamsBossArenaEditorState : ICombatArenaEditorState
  {
    private readonly CombatArenaEditorModel _editorModel;

    public ICombatArenaEditorState EditGrid() => new GridArenaEditorState(_editorModel);

    public ICombatArenaEditorState EditUnits() => new TeamsUnitArenaEditorState(_editorModel);

    public ICombatArenaEditorState EditBoss() => this;

    public bool HandleLeftMouseClick(HexCoordinates position, CombatArenaEditorMapModel model) => false;

    public bool HandleRightMouseClick(HexCoordinates position, CombatArenaEditorMapModel model) => false;

    public bool HandleMiddleMouseClick(HexCoordinates position, CombatArenaEditorMapModel model) => false;
    
    public TeamsBossArenaEditorState(CombatArenaEditorModel editorModel) => _editorModel = editorModel;
  }
}