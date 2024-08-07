using System;
using System.Collections.Generic;
using Mork.HexGrid.Render.Unity.Functions;
using Motk.HexGrid.Core.Descriptors;
using UnityEditor;
using UnityEngine;

namespace Motk.Editor.CombatArenaEditor.Map
{
  public class CombatArenaEditorMapPresenter : IDisposable
  {
    private readonly IHexGridFunctions _hexGridFunctions;
    private readonly CombatArenaEditorModel _editorModel;
    private readonly CombatArenaEditorMapModel _model;
    private readonly CombatArenaEditorMapView _view;
    private readonly GameObject _cameraPrefab;
    private readonly GameObject[] _heroPrefabs;
    private readonly GameObject[] _unitPrefabs;

    public void Initialize()
    {
      _view.AddLeftMouseButtonClickedHandler(View_OnLeftMouseButtonClicked);
      _view.AddRightMouseButtonClickedHandler(View_OnRightMouseButtonClicked);
      _view.AddMiddleMouseButtonClickedHandler(View_OnMiddleMouseButtonClicked);
      
      _editorModel.ArenaPrefab.Changed += Model_OnArenaPrefabChanged;
      _editorModel.GridOffset.Changed += Model_OnOffsetChanged;
      _editorModel.HexRadius.Changed += Model_OnHexRadiusChanged;
      _model.Nodes.ItemAdded += Model_OnNodesItemAdded;
      _model.Nodes.ItemRemoved += Model_OnNodesItemRemoved;
      _model.Nodes.Cleaned += Model_OnNodesCleaned;
      
      for (byte index = 0; index < _editorModel.Teams.Count; index++)
      {
        var side = _editorModel.Teams[index];
        side.UnitPositions.ItemAdded += Model_OnTeamUnitAdded;
        side.UnitPositions.ItemRemoved += Model_OnTeamUnitRemoved;
        side.UnitPositions.Cleaned += Model_OnTeamUnitsCleaned;
        var sideIndex = index;
        side.IsActive.Changed += (_, newValue) => Model_TeamIsActiveChanged(sideIndex, newValue);
      }
    }

    public void Dispose()
    {
      _view.RemoveLeftMouseButtonClickedHandler(View_OnLeftMouseButtonClicked);
      _view.RemoveRightMouseButtonClickedHandler(View_OnRightMouseButtonClicked);
      _view.RemoveMiddleMouseButtonClickedHandler(View_OnMiddleMouseButtonClicked);

      _editorModel.ArenaPrefab.Changed -= Model_OnArenaPrefabChanged;
      _editorModel.GridOffset.Changed -= Model_OnOffsetChanged;
      _editorModel.HexRadius.Changed -= Model_OnHexRadiusChanged;
      _model.Nodes.ItemAdded -= Model_OnNodesItemAdded;
      _model.Nodes.ItemRemoved -= Model_OnNodesItemRemoved;
      _model.Nodes.Cleaned -= Model_OnNodesCleaned;

      foreach (var side in _editorModel.Teams)
      {
        side.UnitPositions.ItemAdded -= Model_OnTeamUnitAdded;
        side.UnitPositions.ItemRemoved -= Model_OnTeamUnitRemoved;
        side.UnitPositions.Cleaned += Model_OnTeamUnitsCleaned;
      }
      
      _view.Dispose();
    }
    
    private bool View_OnLeftMouseButtonClicked(Vector2 mousePosition)
    {
      var gridPosition = MousePositionToGridPosition(mousePosition);

      if(!gridPosition.HasValue)
        return false;

      return _editorModel.State.Value.HandleLeftMouseClick(gridPosition.Value, _model);
    }

    private bool View_OnRightMouseButtonClicked(Vector2 mousePosition)
    {
      var gridPosition = MousePositionToGridPosition(mousePosition);

      if(!gridPosition.HasValue)
        return false;

      return _editorModel.State.Value.HandleRightMouseClick(gridPosition.Value, _model);
    }

    private bool View_OnMiddleMouseButtonClicked(Vector2 mousePosition)
    {
      var gridPosition = MousePositionToGridPosition(mousePosition);
      
      if(!gridPosition.HasValue)
        return false;
      
      return _editorModel.State.Value.HandleMiddleMouseClick(gridPosition.Value, _model);
    }
    
    private void Model_OnArenaPrefabChanged(GameObject? oldValue, GameObject? newValue)
    {
      _view.ArenaPrefab = newValue;
      _editorModel.ArenaInstance.Value = _view.ArenaInstance;
      _view.AddCamera(_cameraPrefab);
    }

    private void Model_OnNodesItemAdded(HexGridNode newItem, int index) => UpdateNodesOnView();

    private void Model_OnNodesItemRemoved(HexGridNode oldItem, int index) => UpdateNodesOnView();

    private void Model_OnNodesCleaned() => _view.RemoveAllNodes();

    private void Model_OnTeamUnitAdded(HexCoordinates newItem, int index) => UpdateUnitsOnView();

    private void Model_OnTeamUnitRemoved(HexCoordinates oldItem, int unitIndex) => UpdateUnitsOnView();

    private void Model_OnTeamUnitsCleaned() => UpdateUnitsOnView();
    
    private void Model_OnHexRadiusChanged(float oldValue, float newValue)
    {
      UpdateNodesOnView();
      UpdateUnitsOnView();
    }
    
    private void Model_OnOffsetChanged(Vector3 oldValue, Vector3 newValue)
    {
      UpdateNodesOnView();
      UpdateUnitsOnView();
    }
    
    private void Model_TeamIsActiveChanged(byte teamIndex, bool newValue)
    {
      if (newValue)
      {
        var id = $"Hero_{teamIndex}";
        _view.AddHero(new MapHeroViewModel(id, teamIndex, _heroPrefabs[teamIndex]));
      }
      else
      {
        _view.RemoveHero(teamIndex);
      }
    }
    
    private void UpdateNodesOnView()
    {
      _view.RemoveAllNodes();
      foreach (var nodeModel in _model.Nodes)
      {
        var worldPosition = GridPositionToWorldPosition(nodeModel.Coordinates);
        var nodeViewModel = new HexNodeViewModel(worldPosition, nodeModel.IsWalkable, _editorModel.HexRadius.Value);
        _view.AddNode(nodeModel.Coordinates, nodeViewModel);
      }
    }
    
    private void UpdateUnitsOnView()
    {
      var unitsViewModels = new List<MapUnitViewModel>();

      for(var teamIndex = 0; teamIndex < _editorModel.Teams.Count; teamIndex++)
      {
        var team = _editorModel.Teams[teamIndex];
        for(var unitIndex = 0; unitIndex < team.UnitPositions.Count; unitIndex++)
        {
          var unitPosition = team.UnitPositions.Collection[unitIndex];
          var worldPosition = GridPositionToWorldPosition(unitPosition);
          var id = $"Unit_({teamIndex}:{unitIndex})_{unitPosition}";
          var viewModel = new MapUnitViewModel(id, worldPosition, Vector3.zero, _unitPrefabs[teamIndex]);
          unitsViewModels.Add(viewModel);
        }
      }

      _view.Units = unitsViewModels;
    }
    
    private Vector3 GridPositionToWorldPosition(HexCoordinates hexCoordinates)
    {
      return _hexGridFunctions.ToLocal(hexCoordinates, _editorModel.HexRadius.Value) + _editorModel.GridOffset.Value;
    }
    
    private HexCoordinates? MousePositionToGridPosition(Vector2 mousePosition)
    {
      var gridPlane = new Plane(Vector3.zero, Vector3.right, Vector3.forward);
      var ray = HandleUtility.GUIPointToWorldRay(mousePosition);

      if(!gridPlane.Raycast(ray, out var distance))
        return null;
      
      var contactPoint = ray.GetPoint(distance);
      var pointWithoutOffset = contactPoint - _editorModel.GridOffset.Value;
        
      var radius = _editorModel.HexRadius.Value;
      return _hexGridFunctions.ToHexCoordinates(pointWithoutOffset, radius);
    }
    
    public CombatArenaEditorMapPresenter(CombatArenaEditorModel editorModel, CombatArenaEditorMapModel model,
      GameObject cameraPrefab, GameObject[] heroPrefabs, GameObject[] unitPrefabs, IHexGridFunctions hexGridFunctions)
    {
      _editorModel = editorModel;
      _view = new CombatArenaEditorMapView(hexGridFunctions);
      _cameraPrefab = cameraPrefab;
      _heroPrefabs = heroPrefabs;
      _unitPrefabs = unitPrefabs;
      _hexGridFunctions = hexGridFunctions;
      _model = model;
    }
  }
}