using Motk.Editor.CombatArenaEditor.Map;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Editor.CombatArenaEditor.States
{
  public class TeamsUnitArenaEditorState : ICombatArenaEditorState
  {
    private readonly CombatArenaEditorModel _editorModel;
    
    public ICombatArenaEditorState EditGrid() => new GridArenaEditorState(_editorModel);

    public ICombatArenaEditorState EditUnits() => this;

    public ICombatArenaEditorState EditBoss() => new TeamsBossArenaEditorState(_editorModel);

    public bool HandleLeftMouseClick(HexCoordinates position, CombatArenaEditorMapModel model)
    {
      if(_editorModel.SelectedTeamIndex.Value == -1)
        return false;
      
      var selectedSide = _editorModel.Teams[_editorModel.SelectedTeamIndex.Value];

      if(!selectedSide.IsActive.Value)
        return false;

      if(selectedSide.UnitPositions.Contains(position))
      {
        selectedSide.UnitPositions.Remove(position);
        return true;
      }
      
      foreach(var team in _editorModel.Teams)
        team.UnitPositions.Remove(p => p.Equals(position));

      selectedSide.UnitPositions.Add(position);
      return true;
    }

    public bool HandleRightMouseClick(HexCoordinates position, CombatArenaEditorMapModel model) => false;

    public bool HandleMiddleMouseClick(HexCoordinates position, CombatArenaEditorMapModel model) => false;
    
    public TeamsUnitArenaEditorState(CombatArenaEditorModel editorModel) => _editorModel = editorModel;
  }
}