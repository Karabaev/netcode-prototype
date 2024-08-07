using System;
using Motk.Editor.CombatArenaEditor.States;
using Motk.Editor.CombatArenaEditor.Window.View;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Motk.Editor.CombatArenaEditor.Window
{
  public class CombatArenaEditorWindowPresenter
  {
    private readonly CombatArenaEditorModel _editorModel;
    private readonly CombatArenaEditorWindowModel _model;
    private readonly CombatArenaEditorWindowView _view;

    public void Initialize()
    {
      _view.IdChanged += View_OnArenaIdChanged;
      _view.ArenaPrefabChanged += View_OnArenaPrefabChanged;
      _view.GridOffsetChanged += View_OnGridOffsetChanged;
      _view.NodeSizeChanged += View_OnGridNodeSizeChanged;
      _view.NoneButtonClicked += View_OnNoneButtonClicked;
      _view.GridButtonClicked += View_OnGridButtonClicked;
      _view.SidesButtonClicked += View_OnSidesButtonClicked;
      _view.SaveButtonClicked += View_OnSaveButtonClicked;
      _view.LoadButtonClicked += View_OnLoadButtonClicked;
      _view.TeamRowClicked += View_OnTeamRowClicked;
      _view.SideActiveChanged += View_OnTeamActiveChanged;
      _view.SideBossChanged += View_OnTeamBossChanged;
      _view.UnitSideModeButtonClicked += View_OnUnitTeamModeButtonClicked;

      _editorModel.ArenaId.Changed += Model_OnArenaIdChanged;
      _editorModel.ArenaPrefab.Changed += Model_OnArenaPrefabChanged;
      _editorModel.State.Changed += Model_OnStateChanged;
      _editorModel.HexRadius.Changed += Model_OnNodeSizeChanged;
      _editorModel.GridOffset.Changed += Model_OnGridOffsetChanged;
      _editorModel.SelectedTeamIndex.Changed += Model_OnSelectedSideIndexChanged;

      _model.Mode.Changed += Model_OnModeChanged;
      _model.TeamsMode.Changed += Model_OnTeamsModeChanged;
      _model.Message.Changed += Model_OnMessageChanged;
      
      for (var index = 0; index < _editorModel.Teams.Count; index++)
      {
        var side = _editorModel.Teams[index];
        var sideIndex = index;
        side.IsActive.Changed += (_, newValue) => Model_SideIsActiveChanged(sideIndex, newValue);
        side.IsBoss.Changed += (_, newValue) => Model_SideIsBossChanged(sideIndex, newValue);
      }
      
      Model_OnModeChanged(CombatArenaEditorMode.Grid, _model.Mode.Value);
      Model_OnSelectedSideIndexChanged(-1, _editorModel.SelectedTeamIndex.Value);
      Model_OnTeamsModeChanged(CombatArenaEditorTeamsMode.Unit, _model.TeamsMode.Value);
      Model_OnGridOffsetChanged(Vector3.zero, Vector3.zero);
      Model_OnStateChanged(null!, _editorModel.State.Value);
    }

    public void Dispose()
    {
      _view.IdChanged += View_OnArenaIdChanged;
      _view.ArenaPrefabChanged += View_OnArenaPrefabChanged;
      _view.GridOffsetChanged -= View_OnGridOffsetChanged;
      _view.NodeSizeChanged -= View_OnGridNodeSizeChanged;

      _view.NoneButtonClicked -= View_OnNoneButtonClicked;
      _view.GridButtonClicked -= View_OnGridButtonClicked;
      _view.SidesButtonClicked -= View_OnSidesButtonClicked;
      _view.SaveButtonClicked -= View_OnSaveButtonClicked;
      _view.LoadButtonClicked -= View_OnLoadButtonClicked;
      _view.TeamRowClicked -= View_OnTeamRowClicked;
      _view.SideActiveChanged -= View_OnTeamActiveChanged;
      _view.SideBossChanged -= View_OnTeamBossChanged;
      _view.UnitSideModeButtonClicked -= View_OnUnitTeamModeButtonClicked;
      
      _editorModel.ArenaId.Changed -= Model_OnArenaIdChanged;
      _editorModel.ArenaPrefab.Changed -= Model_OnArenaPrefabChanged;
      _editorModel.State.Changed -= Model_OnStateChanged;
      _editorModel.HexRadius.Changed -= Model_OnNodeSizeChanged;
      _editorModel.GridOffset.Changed -= Model_OnGridOffsetChanged;
      _editorModel.SelectedTeamIndex.Changed -= Model_OnSelectedSideIndexChanged;

      _model.Mode.Changed -= Model_OnModeChanged;
      _model.TeamsMode.Changed -= Model_OnTeamsModeChanged;
      _model.Message.Changed -= Model_OnMessageChanged;
    }
    
    private void View_OnArenaIdChanged(ChangeEvent<string> evt) => _editorModel.ArenaId.Value = evt.newValue.Trim();

    private void View_OnArenaPrefabChanged(ChangeEvent<Object> evt) => _editorModel.ArenaPrefab.Value = (GameObject)evt.newValue;

    private void View_OnNoneButtonClicked() => _model.Mode.Value = CombatArenaEditorMode.None;

    private void View_OnGridButtonClicked() => _model.Mode.Value = CombatArenaEditorMode.Grid;

    private void View_OnSidesButtonClicked() => _model.Mode.Value = CombatArenaEditorMode.Teams;

    private void View_OnLoadButtonClicked() => _model.LoadButtonClicked.Invoke();

    private void View_OnSaveButtonClicked() => _model.SaveButtonClicked.Invoke();

    private void View_OnTeamRowClicked(sbyte teamIndex) => _editorModel.SelectedTeamIndex.Value = teamIndex;

    private void View_OnTeamActiveChanged(sbyte teamIndex, bool isActive) => _editorModel.Teams[teamIndex].IsActive.Value = isActive;

    private void View_OnTeamBossChanged(sbyte teamIndex, bool isBoss) => _editorModel.Teams[teamIndex].IsBoss.Value = isBoss;

    private void View_OnUnitTeamModeButtonClicked() => _model.TeamsMode.Value = CombatArenaEditorTeamsMode.Unit;

    private void View_OnGridOffsetChanged(ChangeEvent<Vector2> evt)
    {
      var newValue = evt.newValue;
      _editorModel.GridOffset.Value = new Vector3(newValue.x, 0.0f, newValue.y);
    }

    private void View_OnGridNodeSizeChanged(ChangeEvent<float> evt) => _editorModel.HexRadius.Value = evt.newValue;

    private void Model_OnModeChanged(CombatArenaEditorMode oldValue, CombatArenaEditorMode newValue)
    {
      switch (newValue)
      {
        case CombatArenaEditorMode.None:
          _editorModel.State.Value = new NoneArenaEditorState(_editorModel);
          _model.TeamsMode.SetValueWithoutNotify(CombatArenaEditorTeamsMode.None);
          return;
        case CombatArenaEditorMode.Grid:
          _editorModel.State.Value = new GridArenaEditorState(_editorModel);
          _model.TeamsMode.SetValueWithoutNotify(CombatArenaEditorTeamsMode.None);
          break;
        default:
          _editorModel.State.Value = new TeamsUnitArenaEditorState(_editorModel);
          break;
      }
    }

    private void Model_OnSelectedSideIndexChanged(sbyte oldValue, sbyte newValue) => _view.SelectedTeamIndex = newValue;

    private void Model_OnTeamsModeChanged(CombatArenaEditorTeamsMode oldValue, CombatArenaEditorTeamsMode newValue)
    {
      if(newValue == CombatArenaEditorTeamsMode.None)
        return;

      if(_editorModel.State.Value is GridArenaEditorState)
        throw new Exception($"{nameof(GridArenaEditorState)} does not support sides mode");

      _editorModel.State.Value = newValue switch
      {
        CombatArenaEditorTeamsMode.Unit => new TeamsUnitArenaEditorState(_editorModel),
        CombatArenaEditorTeamsMode.Boss => new TeamsBossArenaEditorState(_editorModel),
        _ => throw new ArgumentOutOfRangeException(nameof(newValue), newValue, null)
      };
    }

    private void Model_OnGridOffsetChanged(Vector3 oldValue, Vector3 newValue) => _view.GridOffset = new Vector2(newValue.x, newValue.z);

    private void Model_OnStateChanged(ICombatArenaEditorState oldValue, ICombatArenaEditorState newValue)
    {
      var isNone = newValue is NoneArenaEditorState;
      var isGrid = newValue is GridArenaEditorState;
      
      _view.IsNoneButtonSelected = isNone;
      _view.IsGridButtonSelected = isGrid;
      _view.IsGridContentVisible = isGrid;

      _view.IsTeamsButtonSelected = !isGrid && !isNone;
      _view.IsTeamsContentVisible = !isGrid && !isNone;
      
      _view.IsUnitTeamModeButtonSelected = newValue is TeamsUnitArenaEditorState;
    }

    private void Model_OnMessageChanged(string oldValue, string newValue) => _view.Message = newValue;

    private void Model_OnArenaPrefabChanged(GameObject? oldValue, GameObject? newValue) => _view.ArenaPrefab = newValue;

    private void Model_OnArenaIdChanged(string oldValue, string newValue) => _view.Id = newValue;

    private void Model_OnNodeSizeChanged(float oldValue, float newValue) => _view.NodeSize = newValue;

    private void Model_SideIsActiveChanged(int sideIndex, bool newValue) => _view.SetTeamIsActive(sideIndex, newValue);

    private void Model_SideIsBossChanged(int sideIndex, bool newValue) => _view.SetTeamIsBoss(sideIndex, newValue);
    
    public CombatArenaEditorWindowPresenter(CombatArenaEditorModel editorModel,
      CombatArenaEditorWindowModel model, CombatArenaEditorWindow window)
    {
      _editorModel = editorModel;
      _model = model;
      _view = new CombatArenaEditorWindowView(window.rootVisualElement);
    }
  }
}