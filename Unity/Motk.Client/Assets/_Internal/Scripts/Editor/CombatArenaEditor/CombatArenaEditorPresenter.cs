using System;

namespace Motk.Editor.CombatArenaEditor
{
  public class CombatArenaEditorPresenter : IDisposable
  {
    private readonly CombatArenaEditorModel _model;
    private readonly CombatArenaEditorPreferences _preferences;

    // private readonly CombatArenaEditorWindowModel _windowModel;
    // private readonly CombatArenaEditorWindowPresenter _windowPresenter;

    // private readonly CombatArenaEditorMapModel _mapModel;
    // private readonly CombatArenaEditorMapPresenter _mapPresenter;
    
    public void Initialize()
    {
      // _windowPresenter.Initialize();
      // _mapPresenter.Initialize();

      // _windowModel.SaveButtonClicked.Invoked += Model_OnSaveButtonClicked;
      // _windowModel.LoadButtonClicked.Invoked += Model_OnLoadButtonClicked;
    }
    
    public void Dispose()
    {
      // _windowModel.SaveButtonClicked.Invoked -= Model_OnSaveButtonClicked;
      // _windowModel.LoadButtonClicked.Invoked -= Model_OnLoadButtonClicked;
      
      // _windowPresenter.Dispose();
      // _mapPresenter.Dispose();
    }

    private void Model_OnSaveButtonClicked()
    {
      // todokmo
    }

    private void Model_OnLoadButtonClicked()
    {
      // todokmo
    }
  }
}