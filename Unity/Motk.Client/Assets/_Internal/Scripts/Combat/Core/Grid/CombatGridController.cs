﻿using System;

namespace Motk.Combat.Client.Core.Grid
{
  public class CombatGridController : IDisposable
  {
    private readonly CombatGridState _combatGridState;
    private readonly CombatState _combatState;

    public CombatGridController(CombatGridState combatGridState, CombatState combatState)
    {
      _combatGridState = combatGridState;
      _combatState = combatState;
    }

    public void Start()
    {
      
    }

    void IDisposable.Dispose()
    {
      
    }
  }
}