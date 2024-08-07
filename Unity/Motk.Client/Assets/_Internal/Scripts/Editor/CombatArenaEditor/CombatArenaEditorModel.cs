using System.Collections.Generic;
using com.karabaev.reactivetypes.Action;
using com.karabaev.reactivetypes.Property;
using Motk.Editor.CombatArenaEditor.States;
using UnityEngine;

namespace Motk.Editor.CombatArenaEditor
{
  public class CombatArenaEditorModel
  {
    public ReactiveProperty<string> ArenaId { get; } = new(string.Empty);

    public ReactiveProperty<GameObject?> ArenaPrefab { get; } = new(null);
    
    // todokmo проверить, есть ли смысл отдельно инстанс тащить
    public ReactiveProperty<GameObject?> ArenaInstance { get; } = new(null);

    public ReactiveProperty<ICombatArenaEditorState> State { get; }

    public ReactiveProperty<float> HexRadius { get; }

    public ReactiveProperty<Vector3> GridOffset { get; }
    
    public ReactiveProperty<sbyte> SelectedTeamIndex { get; }
    
    public IReadOnlyList<CombatArenaEditorTeamModel> Teams { get; }

    public ReactiveAction<string> ErrorOccured { get; } = new();

    public CombatArenaEditorModel(float hexRadius, Vector3 gridOffset, sbyte selectedTeamIndex)
    {
      State = new ReactiveProperty<ICombatArenaEditorState>(new NoneArenaEditorState(this));
      HexRadius = new ReactiveProperty<float>(hexRadius);
      GridOffset = new ReactiveProperty<Vector3>(gridOffset);
      SelectedTeamIndex = new ReactiveProperty<sbyte>(selectedTeamIndex);
      Teams = new CombatArenaEditorTeamModel[] { new(), new(), new(), new() };
    }
  }
}