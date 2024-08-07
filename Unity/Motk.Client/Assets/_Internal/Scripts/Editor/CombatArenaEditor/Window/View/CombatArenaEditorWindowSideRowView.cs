using System;
using UnityEngine.UIElements;

namespace Motk.Editor.CombatArenaEditor.Window.View
{
  public class CombatArenaEditorWindowSideRowView
  {
    private const string HighlightedClass = "highlighted";
    
    private readonly VisualElement _root;
    private readonly Toggle _activeToggle;
    private readonly Toggle _bossToggle;

    private Action<sbyte>? _clickedAction;

    private readonly sbyte _index;

    public bool Highlighted
    {
      set
      {
        if(value)
          _root.AddToClassList(HighlightedClass);
        else
          _root.RemoveFromClassList(HighlightedClass);
      }
    }

    public bool IsActive
    {
      set => _activeToggle.SetValueWithoutNotify(value);
    }
    
    public bool IsBoss
    {
      set => _bossToggle.SetValueWithoutNotify(value);
    }

    public event EventCallback<ChangeEvent<bool>>? ActiveChanged
    {
      add => _activeToggle.RegisterValueChangedCallback(value);
      remove => _activeToggle.UnregisterValueChangedCallback(value);
    }
    
    public event EventCallback<ChangeEvent<bool>>? BossChanged
    {
      add => _bossToggle.RegisterValueChangedCallback(value);
      remove => _bossToggle.UnregisterValueChangedCallback(value);
    }
    
    public void AddClickedAction(Action<sbyte>? action) => _clickedAction += action;

    public void RemoveClickedAction(Action<sbyte>? action) => _clickedAction -= action;

    public void Dispose()
    {
      _root.UnregisterCallback<ClickEvent>(OnClicked);

      _clickedAction = null;
    }

    private void OnClicked(ClickEvent evt) => _clickedAction?.Invoke(_index);

    public CombatArenaEditorWindowSideRowView(VisualElement root, sbyte index)
    {
      _root = root;
      _index = index;
      _activeToggle = root.Q<Toggle>("active-toggle");
      _bossToggle = root.Q<Toggle>("boss-toggle");

      _root.RegisterCallback<ClickEvent>(OnClicked);
    }
  }
}