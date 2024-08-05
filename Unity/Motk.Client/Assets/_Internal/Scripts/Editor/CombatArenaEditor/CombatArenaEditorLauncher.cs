using UnityEditor.SceneManagement;

namespace Motk.Editor.CombatArenaEditor
{
  public static class CombatArenaEditorLauncher
  {
    private const string EditorSceneName = "CombatArenaEditor";
    private const string EditorScenePath =
      "Assets/_Internal/Scenes/Editor/CombatArenaEditor" + EditorSceneName + ".unity";

    private static CombatArenaEditorController? _controller;
    
    public static void LaunchEditor()
    {
      _controller?.Dispose();
      var preferences = CombatArenaEditorPreferences.Instance;
      _controller = new CombatArenaEditorController(EditorSceneName, preferences);
      EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
      EditorSceneManager.OpenScene(EditorScenePath);
    } 
  }
}