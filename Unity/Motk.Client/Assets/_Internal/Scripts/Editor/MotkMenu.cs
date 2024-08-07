using Motk.Editor.CombatArenaEditor;
using Motk.Editor.CombatArenaEditor.EntryPoint;
using UnityEditor;
using UnityEngine;

namespace Motk.Editor
{
  public class MotkMenu : MonoBehaviour
  {
    [MenuItem("Motk/Combat arena editor")]
    private static void OpenCombatArenaEditor() => CombatArenaEditorLauncher.LaunchEditor();
  }
}