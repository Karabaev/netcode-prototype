using System;
using Mork.HexGrid.Render.Unity.Functions;
using Motk.Editor.CombatArenaEditor.Map;
using Motk.Editor.CombatArenaEditor.Window;

namespace Motk.Editor.CombatArenaEditor
{
  public class CombatArenaEditorPresenter : IDisposable
  {
    private readonly CombatArenaEditorWindowModel _windowModel;
    private readonly CombatArenaEditorWindowPresenter _windowPresenter;
    private readonly CombatArenaEditorMapPresenter _mapPresenter;

    public void Initialize()
    {
      _windowPresenter.Initialize();
      _mapPresenter.Initialize();

      _windowModel.SaveButtonClicked.Invoked += Model_OnSaveButtonClicked;
      _windowModel.LoadButtonClicked.Invoked += Model_OnLoadButtonClicked;
    }

    public void Dispose()
    {
      _windowModel.SaveButtonClicked.Invoked -= Model_OnSaveButtonClicked;
      _windowModel.LoadButtonClicked.Invoked -= Model_OnLoadButtonClicked;
      
      _windowPresenter.Dispose();
      _mapPresenter.Dispose();
    }

    private void Model_OnSaveButtonClicked()
    {
      // todokmo
    }

    private void Model_OnLoadButtonClicked()
    {
      // todokmo
    }

    public CombatArenaEditorPresenter(CombatArenaEditorModel editorModel, CombatArenaEditorWindow window,
      CombatArenaEditorPreferences editorPreferences)
    {
      _windowModel = new CombatArenaEditorWindowModel(CombatArenaEditorMode.None, CombatArenaEditorTeamsMode.Unit);
      _windowPresenter = new CombatArenaEditorWindowPresenter(editorModel, _windowModel, window);
      var mapModel = new CombatArenaEditorMapModel();
      _mapPresenter = new CombatArenaEditorMapPresenter(editorModel, mapModel, editorPreferences.CameraPrefab,
        editorPreferences.HeroPrefabs, editorPreferences.UnitPrefabs, new FlatToppedHexGridFunctions());
    }
  }
}