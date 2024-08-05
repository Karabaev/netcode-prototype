using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Motk.Editor.CombatArenaEditor
{
  [CreateAssetMenu(menuName = "Motk/Editor/Combat arena editor preferences")]
  public class CombatArenaEditorPreferences : ScriptableObject
  {
    [field: SerializeField]
    public VisualTreeAsset EditorWindowTree { get; private set; } = null!;
    
    private static CombatArenaEditorPreferences? _instance;

    public static CombatArenaEditorPreferences Instance
    {
      get
      {
        if (_instance != null)
          return _instance;

        var foundAssets = AssetDatabase.FindAssets($"{nameof(CombatArenaEditorPreferences)}");
        if (foundAssets.Length == 0)
          throw new NullReferenceException($"{nameof(CombatArenaEditorPreferences)} not found in the project");

        if (foundAssets.Length > 1)
          throw new InvalidOperationException(
            $"There are 2 or more instances of {nameof(CombatArenaEditorPreferences)} in the project");

        var loadedAsset =
          AssetDatabase.LoadAssetAtPath<CombatArenaEditorPreferences>(AssetDatabase.GUIDToAssetPath(foundAssets[0]));
        return _instance ??= loadedAsset;
      }
    }
  }
}