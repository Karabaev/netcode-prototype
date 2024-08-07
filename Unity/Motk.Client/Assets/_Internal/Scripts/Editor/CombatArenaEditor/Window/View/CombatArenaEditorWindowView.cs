using System;
using com.karabaev.utilities;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Motk.Editor.CombatArenaEditor.Window.View
{
  public class CombatArenaEditorWindowView
  {
    private readonly TextField _idField;
    private readonly ObjectField _arenaPrefabField;
    private readonly Button _noneButton;
    private readonly Button _gridButton;
    private readonly VisualElement _gridTabContent;
    private readonly FloatField _nodeSizeField;
    private readonly Vector2Field _gridOffsetField;
    private readonly Button _teamsButton;
    private readonly VisualElement _teamsTabContent;
    private readonly CombatArenaEditorWindowSideRowView[] _teamRows;
    private readonly Button _unitTeamModeButton;
    private readonly Button _loadButton;
    private readonly Button _saveButton;
    private readonly Label _messageLabel;
    
    public bool IsNoneButtonSelected
    {
      set => _noneButton.SetEnabled(!value);
    }
    
    public bool IsGridButtonSelected
    {
      set => _gridButton.SetEnabled(!value);
    }
    
    public bool IsTeamsButtonSelected
    {
      set => _teamsButton.SetEnabled(!value);
    }
    
    public bool IsGridContentVisible
    {
      set => _gridTabContent.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public bool IsTeamsContentVisible
    {
      set => _teamsTabContent.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public string Message
    {
      set => _messageLabel.text = value;
    }
    
    public int SelectedTeamIndex
    {
      set
      {
        for(var i = 0; i < _teamRows.Length; i++)
          _teamRows[i].Highlighted = i == value;
      } 
    }
    
    public bool IsUnitTeamModeButtonSelected
    {
      set => _unitTeamModeButton.SetEnabled(!value);
    }
    
    public Vector2 GridOffset
    {
      set => _gridOffsetField.SetValueWithoutNotify(value);
    }

    public string Id
    {
      set => _idField.SetValueWithoutNotify(value);
    }

    public float NodeSize
    {
      set => _nodeSizeField.SetValueWithoutNotify(value);
    }
    
    public GameObject? ArenaPrefab
    {
      set => _arenaPrefabField.SetValueWithoutNotify(value);
    }
    
    public event EventCallback<ChangeEvent<string>>? IdChanged
    {
      add => _idField.RegisterValueChangedCallback(value);
      remove => _idField.UnregisterValueChangedCallback(value);
    } 
    
    public event EventCallback<ChangeEvent<Object>>? ArenaPrefabChanged
    {
      add => _arenaPrefabField.RegisterValueChangedCallback(value);
      remove => _arenaPrefabField.UnregisterValueChangedCallback(value);
    }

    public event EventCallback<ChangeEvent<Vector2>>? GridOffsetChanged
    {
      add => _gridOffsetField.RegisterValueChangedCallback(value);
      remove => _gridOffsetField.UnregisterValueChangedCallback(value);
    }
    
    public event EventCallback<ChangeEvent<float>>? NodeSizeChanged
    {
      add => _nodeSizeField.RegisterValueChangedCallback(value);
      remove => _nodeSizeField.UnregisterValueChangedCallback(value);
    } 

    public event Action? NoneButtonClicked
    {
      add => _noneButton.clicked += value;
      remove => _noneButton.clicked -= value;
    }
    
    public event Action? GridButtonClicked
    {
      add => _gridButton.clicked += value;
      remove => _gridButton.clicked -= value;
    }

    public event Action? SidesButtonClicked
    {
      add => _teamsButton.clicked += value;
      remove => _teamsButton.clicked -= value;
    }

    public event Action<sbyte>? TeamRowClicked
    {
      add => _teamRows.ForEach(s => s.AddClickedAction(value));
      remove => _teamRows.ForEach(s => s.RemoveClickedAction(value));
    }

    public event Action<sbyte, bool>? SideActiveChanged;

    public event Action<sbyte, bool>? SideBossChanged;

    public event Action? UnitSideModeButtonClicked
    {
      add => _unitTeamModeButton.clicked += value;
      remove => _unitTeamModeButton.clicked -= value;
    }

    public event Action? LoadButtonClicked
    {
      add => _loadButton.clicked += value;
      remove => _loadButton.clicked -= value;
    }
    
    public event Action? SaveButtonClicked
    {
      add => _saveButton.clicked += value;
      remove => _saveButton.clicked -= value;
    }
    
    public void SetTeamIsActive(int teamIndex, bool value) => _teamRows[teamIndex].IsActive = value;

    public void SetTeamIsBoss(int sideIndex, bool value) => _teamRows[sideIndex].IsBoss = value;
    
    public CombatArenaEditorWindowView(VisualElement root)
    {
      _idField = root.Q<TextField>("id-field");
      _arenaPrefabField = root.Q<ObjectField>("arenaPrefab-field");
      _arenaPrefabField.objectType = typeof(GameObject);
      _arenaPrefabField.allowSceneObjects = false;
      
      _noneButton = root.Q<Button>("none-tabButton"); 
      _gridButton = root.Q<Button>("grid-tabButton"); 
      _teamsButton = root.Q<Button>("sides-tabButton"); // todokmo team
      _gridTabContent = root.Q<VisualElement>("grid-content");
      _teamsTabContent = root.Q<VisualElement>("sides-content"); // todokmo team
      _nodeSizeField = _gridTabContent.Q<FloatField>("nodeSize-field");

      _gridOffsetField = _gridTabContent.Q<Vector2Field>("offset-field");

      var sidesPanel = _teamsTabContent.Q<VisualElement>("sides-panel"); // todokmo team
      var sideRows = sidesPanel.Query<VisualElement>(className: "sideRow").ToList(); // todokmo team
      _teamRows = new CombatArenaEditorWindowSideRowView[sideRows.Count];
      for(sbyte i = 0; i < sideRows.Count; i++)
      {
        var teamIndex = i;
        var row = new CombatArenaEditorWindowSideRowView(sideRows[i], i);
        row.ActiveChanged += evt => SideActiveChanged?.Invoke(teamIndex, evt.newValue);
        row.BossChanged += evt => SideBossChanged?.Invoke(teamIndex, evt.newValue);
        _teamRows[i] = row;
      }

      var sideModesPanel = _teamsTabContent.Q<VisualElement>("sidesModes-panel"); // todokmo team
      _unitTeamModeButton = sideModesPanel.Q<Button>("unitMode-button");
      _saveButton = root.Q<Button>("save-button");
      _loadButton = root.Q<Button>("load-button");
      _messageLabel = root.Q<Label>("message-label");
    }
  }
}