using UnityEditor;
using UnityEngine.UIElements;

namespace Motk.Editor.CombatArenaEditor
{
  public class CombatArenaEditorWindow : EditorWindow
  {
    private VisualTreeAsset _visualTree = null!;

    public void Initialize(VisualTreeAsset visualTree)
    {
      _visualTree = visualTree;
    }

    private void CreateGUI()
    {
      var root = _visualTree.Instantiate();
      rootVisualElement.Add(root);
    }
  }
}