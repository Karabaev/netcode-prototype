using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using com.karabaev.utilities;
using com.karabaev.utilities.unity;
using Mork.HexGrid.Render.Unity.Functions;
using Motk.HexGrid.Core.Descriptors;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Motk.Editor.CombatArenaEditor.Map
{
  [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
  public class CombatArenaEditorMapView : IDisposable
  {
    private readonly IHexGridFunctions _hexGridFunctions;
    private readonly Transform _rootObject;
    private readonly Dictionary<HexCoordinates, HexNodeView> _nodes = new();
    private readonly Dictionary<int, GameObject> _heroes = new();
    private readonly List<GameObject> _units = new();
    private readonly List<Func<Vector2, bool>> _leftMouseClickHandlers = new();
    private readonly List<Func<Vector2, bool>> _rightMouseClickHandlers = new();
    private readonly List<Func<Vector2, bool>> _middleMouseClickHandlers = new();
    private GameObject? _cameraInstance;

    public IReadOnlyList<MapUnitViewModel> Units
    {
      set
      {
        _units.ForEach(Object.DestroyImmediate);
        _units.Clear();
        
        foreach(var unit in value)
        {
          var unitObject = (GameObject)PrefabUtility.InstantiatePrefab(unit.Prefab, _rootObject);
          unitObject.name = $"<{unit.Id}>";
          unitObject.transform.position = unit.WorldPosition;
          unitObject.transform.eulerAngles = unit.Rotation;
          _units.Add(unitObject);
        }
      }
    }
    
    public GameObject? ArenaPrefab
    {
      set
      {
        if(ArenaInstance)
          Object.DestroyImmediate(ArenaInstance);

        ArenaInstance = (GameObject)PrefabUtility.InstantiatePrefab(value, _rootObject);
        ArenaInstance.name = $"<Arena>";
        ArenaInstance.transform.position = Vector3.zero;
        ArenaInstance.transform.eulerAngles = Vector3.zero;
      }
    }

    public GameObject? ArenaInstance { get; private set; }
    
    public void AddNode(HexCoordinates coordinates, HexNodeViewModel viewModel)
    {
      var view = new HexNodeView(coordinates, viewModel, _hexGridFunctions);
      _nodes.Add(coordinates, view);
    }

    public void RemoveAllNodes() => _nodes.Clear();
    
    public void AddHero(MapHeroViewModel viewModel)
    {
      var heroObject = (GameObject)PrefabUtility.InstantiatePrefab(viewModel.Prefab, _rootObject);
      heroObject.name = viewModel.Id;
      _heroes.Add(viewModel.TeamIndex, heroObject);
    }

    public void RemoveHero(int teamIndex)
    {
      _heroes.Remove(teamIndex, out var heroObj);
      Object.DestroyImmediate(heroObj);
    }
    
    public void AddCamera(GameObject prefab)
    {
      if(_cameraInstance)
        Object.DestroyImmediate(_cameraInstance!.gameObject);
      
      _cameraInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, _rootObject);
      _cameraInstance.name = "<Camera>";
    }
    
    public void AddLeftMouseButtonClickedHandler(Func<Vector2, bool> handler) => _leftMouseClickHandlers.Add(handler);
    
    public void RemoveLeftMouseButtonClickedHandler(Func<Vector2, bool> handler) => _leftMouseClickHandlers.Remove(handler);

    public void AddRightMouseButtonClickedHandler(Func<Vector2, bool> handler) => _rightMouseClickHandlers.Add(handler);
    
    public void RemoveRightMouseButtonClickedHandler(Func<Vector2, bool> handler) => _rightMouseClickHandlers.Remove(handler);
    
    public void AddMiddleMouseButtonClickedHandler(Func<Vector2, bool> handler) => _middleMouseClickHandlers.Add(handler);
    
    public void RemoveMiddleMouseButtonClickedHandler(Func<Vector2, bool> handler) => _middleMouseClickHandlers.Remove(handler);

    private void OnDrawGizmos(SceneView sceneView)
    {
      _nodes.ForEach(n => n.Value.OnDrawGizmos());
      _units.ForEach(u => GizmosHelper.DrawString(u.name, u.transform.position));
      _heroes.ForEach(h => GizmosHelper.DrawString(h.Value.name, h.Value.transform.position));
    }
    
    private void OnInput(SceneView sceneView)
    {
      var current = Event.current;
      
      if(current.alt || current.control || current.shift || current.type != EventType.MouseDown)
        return;

      var mousePosition = current.mousePosition;

      var handlers = current.button switch
      {
        0 => _leftMouseClickHandlers,
        1 => _rightMouseClickHandlers,
        2 => _middleMouseClickHandlers,
        _ => null
      };

      if(handlers == null)
        return;

      if(HandleMouseClick(mousePosition, handlers))
        current.Use();
    }
    
    private bool HandleMouseClick(Vector2 mousePosition, List<Func<Vector2, bool>> handlers)
    {
      var finishEvent = false;
      foreach(var handler in handlers)
        finishEvent |= handler.Invoke(mousePosition);

      return finishEvent;
    }
    
    public CombatArenaEditorMapView(IHexGridFunctions hexGridFunctions)
    {
      _hexGridFunctions = hexGridFunctions;
      _rootObject = new GameObject("[Root]").transform;
      SceneView.beforeSceneGui += OnInput;
      SceneView.duringSceneGui += OnDrawGizmos;
    }
    
    public void Dispose()
    {
      Object.DestroyImmediate(_rootObject.gameObject);
      SceneView.beforeSceneGui -= OnInput;
      SceneView.duringSceneGui -= OnDrawGizmos;
    }
  }
}