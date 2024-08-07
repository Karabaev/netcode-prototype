using Motk.Editor.CombatArenaEditor.Map;
using Motk.Editor.CombatArenaEditor.States;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Editor.CombatArenaEditor
{
  public class GridArenaEditorState : ICombatArenaEditorState
  {
    private readonly CombatArenaEditorModel _editorModel;
    
    public GridArenaEditorState(CombatArenaEditorModel editorModel) => _editorModel = editorModel;

    public ICombatArenaEditorState EditGrid() => this;

    public ICombatArenaEditorState EditUnits() => new TeamsUnitArenaEditorState(_editorModel);

    public ICombatArenaEditorState EditBoss() => new TeamsBossArenaEditorState(_editorModel);

    public bool HandleLeftMouseClick(HexCoordinates position, CombatArenaEditorMapModel model)
    {
      model.Nodes.Remove(n => n.Coordinates.Equals(position));
      model.Nodes.Add(new HexGridNode(position, true));
      return true;
    }

    public bool HandleRightMouseClick(HexCoordinates position, CombatArenaEditorMapModel model)
    {
      model.Nodes.Remove(n => n.Coordinates.Equals(position));
      model.Nodes.Add(new HexGridNode(position, false));
      return true;
    }

    public bool HandleMiddleMouseClick(HexCoordinates position, CombatArenaEditorMapModel model)
    {
      model.Nodes.Remove(n => n.Coordinates.Equals(position));
      return true;
    }
  }
}