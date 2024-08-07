using UnityEditor;
using UnityEditor.SceneManagement;

namespace Motk.Editor.CombatArenaEditor.EntryPoint
{
  public static class CombatArenaEditorLauncher
  {
    private static CombatArenaEditorController? _controller;
    
    public static void LaunchEditor()
    {
      _controller?.Dispose();
      var preferences = CombatArenaEditorPreferences.Instance;
      _controller = new CombatArenaEditorController(preferences.EditorScene.name, preferences);
      EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
      var scenePath = AssetDatabase.GetAssetOrScenePath(preferences.EditorScene);
      EditorSceneManager.OpenScene(scenePath);
    } 
  }
}