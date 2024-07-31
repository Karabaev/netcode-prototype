using System;
using System.Collections.Generic;
using com.karabaev.utilities;
using JetBrains.Annotations;
using Motk.Client.Combat.Units.Core.Services;

namespace Motk.Client.Combat.Units.Core.Controllers
{
  [UsedImplicitly]
  public class CombatUnitsController : IDisposable
  {
    private readonly CombatState _combatState;
    private readonly ICombatUnitFactory _combatUnitFactory;

    private readonly Dictionary<ushort, TeamUnitsController> _unitsControllers = new();

    public CombatUnitsController(CombatState combatState, ICombatUnitFactory combatUnitFactory)
    {
      _combatState = combatState;
      _combatUnitFactory = combatUnitFactory;
    }
    
    public void Start()
    {
      _combatState.Teams.ItemAdded += State_OnTeamAdded;
      _combatState.Teams.ItemRemoved += State_OnTeamRemoved;
      
      _combatState.Teams.ForEach(t => State_OnTeamAdded(t.Key, t.Value));
    }

    void IDisposable.Dispose()
    {
      _combatState.Teams.ItemAdded -= State_OnTeamAdded;
      _combatState.Teams.ItemRemoved -= State_OnTeamRemoved;
      
      _combatState.Teams.ForEach(t => State_OnTeamRemoved(t.Key, t.Value));
    }

    private void State_OnTeamAdded(ushort key, CombatTeamState newValue)
    {
      var newController = new TeamUnitsController(newValue, _combatUnitFactory);
      _unitsControllers.Add(key, newController);
      newController.Start();
    }

    private void State_OnTeamRemoved(ushort key, CombatTeamState oldValue)
    {
      _unitsControllers.Remove(key, out var controller);
      controller.Dispose();
    }
  }
}