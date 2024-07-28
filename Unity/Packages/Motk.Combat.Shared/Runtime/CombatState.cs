using System.Collections.Generic;

namespace Motk.Combat.Shared
{
  public class CombatState
  {
    public RoundState CurrentRound { get; set; }
  
    public IReadOnlyDictionary<int, TeamState> Teams { get; }
  
    public IEnumerable<UnitState> Units
    {
      get
      {
        foreach(var (_, team) in Teams)
        {
          foreach(var (_, unit) in team.Units)
          {
            yield return unit;
          }
        }
      }
    }
  }
}