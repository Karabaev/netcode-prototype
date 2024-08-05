using Motk.Editor.CombatArenaEditor;
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