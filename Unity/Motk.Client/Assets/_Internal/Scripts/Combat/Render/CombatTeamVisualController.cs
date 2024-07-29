using System;
using System.Collections.Generic;
using com.karabaev.utilities;
using com.karabaev.utilities.unity;
using Mork.HexGrid.Render.Unity;
using Motk.Client.Combat.Units;
using Motk.Client.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Motk.Client.Combat.Render
{
  public class CombatTeamVisualController : IDisposable
  {
    private readonly CombatTeamState _teamState;
    private readonly UnitVisualRegistry _unitVisualRegistry;

    private readonly Dictionary<ushort, CombatUnitVisualView> _unitViews = new();
    private readonly Dictionary<ushort, CombatUnitVisualState> _unitStates = new();

    public CombatTeamVisualController(CombatTeamState teamState, UnitVisualRegistry unitVisualRegistry)
    {
      _teamState = teamState;
      _unitVisualRegistry = unitVisualRegistry;
      
    }

    public void Start()
    {
      _teamState.Units.ForEach(u => State_OnUnitAdded(u.Key, u.Value));
      _teamState.Units.ItemAdded += State_OnUnitAdded;
      _teamState.Units.ItemRemoved += State_OnUnitRemoved;
    }
      
    public void Dispose()
    {
      _teamState.Units.ItemAdded -= State_OnUnitAdded;
      _teamState.Units.ItemRemoved -= State_OnUnitRemoved;
    }
      
    private void State_OnUnitAdded(ushort unitId, CombatUnitState newUnit)
    {
      var visualDescriptor = _unitVisualRegistry.Require(newUnit.DescriptorId);
      var unitView = Object.Instantiate(visualDescriptor.CombatPrefab);
      var visualState = new CombatUnitVisualState(newUnit.Position.Value.ToWorld(0.0f) , Quaternion.identity);
      unitView.Construct(visualState);
      _unitViews.Add(unitId, unitView);
      _unitStates.Add(unitId, visualState);
    }

    private void State_OnUnitRemoved(ushort unitId, CombatUnitState oldUnit)
    {
      _unitViews.Remove(unitId, out var view);
      view.DestroyObject();
      _unitStates.Remove(unitId);
    }
  }
}