using System;
using System.Collections.Generic;
using com.karabaev.utilities;
using Motk.Client.Combat.Units.Core.Services;

namespace Motk.Client.Combat.Units.Core.Controllers
{
  public class TeamUnitsController : IDisposable
  {
    private readonly CombatTeamState _combatTeamState;
    private readonly ICombatUnitFactory _combatUnitFactory;

    private readonly Dictionary<ushort, CombatUnitEntity> _units = new();

    public TeamUnitsController(CombatTeamState combatTeamState, ICombatUnitFactory combatUnitFactory)
    {
      _combatTeamState = combatTeamState;
      _combatUnitFactory = combatUnitFactory;
    }

    public void Start()
    {
      _combatTeamState.Units.ItemAdded += State_OnUnitAdded;
      _combatTeamState.Units.ItemRemoved += State_OnUnitRemoved;
      
      _combatTeamState.Units.ForEach(u => State_OnUnitAdded(u.Key, u.Value));
    }

    public void Dispose()
    {
      _combatTeamState.Units.ItemAdded -= State_OnUnitAdded;
      _combatTeamState.Units.ItemRemoved -= State_OnUnitRemoved;
      
      _combatTeamState.Units.ForEach(u => State_OnUnitRemoved(u.Key, u.Value));
    }

    private async void State_OnUnitAdded(ushort key, CombatUnitState newValue)
    {
      var unit = await _combatUnitFactory.CreateAsync(newValue);
      _units.Add(key, unit);
      unit.Start();
    }

    private void State_OnUnitRemoved(ushort key, CombatUnitState oldValue)
    {
      _units.Remove(key, out var unit);
      unit.Dispose();
    }
  }
}