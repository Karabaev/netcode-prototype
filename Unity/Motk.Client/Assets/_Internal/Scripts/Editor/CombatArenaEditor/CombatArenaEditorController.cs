using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Motk.Editor.CombatArenaEditor
{
  public class CombatArenaEditorController : IDisposable
  {
    private readonly string _sceneName;
    private readonly CombatArenaEditorPreferences _preferences;
    private readonly CombatArenaEditorModel _editorModel;
    private CombatArenaEditorPresenter? _editorPresenter;
    private CombatArenaEditorWindow? _editorWindow;

    private void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
      if (scene.name != _sceneName)
        return;

      EditorSceneManager.sceneOpened -= OnSceneOpened;
      EditorSceneManager.sceneClosed += OnSceneClosed;

      _editorWindow = EditorWindow.GetWindow<CombatArenaEditorWindow>("Combat arena editor");
      _editorWindow.Initialize(_preferences.EditorWindowTree);
      _editorPresenter?.Dispose();
      _editorPresenter = new CombatArenaEditorPresenter();
      _editorModel.ErrorOccured.Invoked -= State_OnErrorOccured;
      _editorModel.ErrorOccured.Invoked += State_OnErrorOccured;
      _editorPresenter.Initialize();
    }

    private void OnSceneClosed(Scene scene)
    {
      if (scene.name != _sceneName)
        return;
      
      Dispose();
    }

    private void State_OnErrorOccured(string errorText)
    {
      EditorUtility.DisplayDialog("Error!", errorText, "Got it");
    }

    public void Dispose()
    {
      EditorSceneManager.sceneOpened -= OnSceneOpened;
      EditorSceneManager.sceneClosed -= OnSceneClosed;
      _editorPresenter?.Dispose();
      _editorPresenter = null;
      // ReSharper disable once Unity.NoNullPropagation
      _editorWindow?.Close();
      _editorWindow = null;
      _editorModel.ErrorOccured.Invoked -= State_OnErrorOccured;
      GC.Collect();
    }

    public CombatArenaEditorController(string sceneName, CombatArenaEditorPreferences preferences)
    {
      _sceneName = sceneName;
      _preferences = preferences;
      _editorModel = new CombatArenaEditorModel();
      
      EditorSceneManager.sceneOpened += OnSceneOpened;
    }
  }
}