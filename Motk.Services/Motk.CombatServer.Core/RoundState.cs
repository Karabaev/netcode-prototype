namespace Motk.CombatServer.Core;

public class RoundState
{
  public int Index { get; }
  
  public UnitIdentifier? ActiveUnit { get; set; }
  
  public List<UnitIdentifier> TurnsQueue { get; }
}