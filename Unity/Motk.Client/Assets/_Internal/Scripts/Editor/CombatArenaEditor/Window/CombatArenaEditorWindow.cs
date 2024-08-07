using UnityEditor;
using UnityEngine.UIElements;

namespace Motk.Editor.CombatArenaEditor.Window
{
  public class CombatArenaEditorWindow : EditorWindow
  {
    private VisualTreeAsset _visualTree = null!;

    private void Awake() => _visualTree = CombatArenaEditorPreferences.Instance.EditorWindowTree;

    private void CreateGUI()
    {
      var root = _visualTree.Instantiate();
      rootVisualElement.Add(root);
    }
  }
}