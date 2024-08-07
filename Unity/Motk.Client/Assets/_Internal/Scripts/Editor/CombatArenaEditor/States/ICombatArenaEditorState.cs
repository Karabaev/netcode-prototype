using Motk.Editor.CombatArenaEditor.Map;
using Motk.HexGrid.Core.Descriptors;

namespace Motk.Editor.CombatArenaEditor.States
{
  public interface ICombatArenaEditorState
  {
    ICombatArenaEditorState EditGrid();

    ICombatArenaEditorState EditUnits();
    
    ICombatArenaEditorState EditBoss();

    bool HandleLeftMouseClick(HexCoordinates position, CombatArenaEditorMapModel model);

    bool HandleRightMouseClick(HexCoordinates position, CombatArenaEditorMapModel model);

    bool HandleMiddleMouseClick(HexCoordinates position, CombatArenaEditorMapModel model);
  }
}