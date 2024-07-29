using System;
using System.Collections.Generic;
using com.karabaev.utilities;
using JetBrains.Annotations;
using Motk.Client.Units;

namespace Motk.Client.Combat.Render
{
  [UsedImplicitly]
  public class CombatTeamsVisualController : IDisposable
  {
    private readonly CombatState _combatState;
    private readonly UnitVisualRegistry _unitVisualRegistry;

    private readonly Dictionary<ushort, CombatTeamVisualController> _teamControllers;

    public CombatTeamsVisualController(CombatState combatState, UnitVisualRegistry unitVisualRegistry)
    {
      _combatState = combatState;
      _unitVisualRegistry = unitVisualRegistry;
      _teamControllers = new Dictionary<ushort, CombatTeamVisualController>();
    }

    public void Start()
    {
      _combatState.Teams.ForEach(t => State_OnTeamAdded(t.Key, t.Value));
      
      _combatState.Teams.ItemAdded += State_OnTeamAdded;
      _combatState.Teams.ItemRemoved += State_OnTeamRemoved;
    }
    
    void IDisposable.Dispose()
    {
      _combatState.Teams.ItemAdded -= State_OnTeamAdded;
      _combatState.Teams.ItemRemoved -= State_OnTeamRemoved;
      _teamControllers.ForEach(o => o.Value.Dispose());
    }

    private void State_OnTeamAdded(ushort teamId, CombatTeamState newValue)
    {
      var teamController = new CombatTeamVisualController(newValue, _unitVisualRegistry);
      _teamControllers.Add(teamId, teamController);
      teamController.Start();
    }

    private void State_OnTeamRemoved(ushort teamId, CombatTeamState oldValue)
    {
      _teamControllers.Remove(teamId, out var observer);
      observer.Dispose();
    }
  }
}