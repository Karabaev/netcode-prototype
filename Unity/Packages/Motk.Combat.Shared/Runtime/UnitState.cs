namespace Motk.Combat.Shared
{
  public class UnitState
  {
    public UnitIdentifier Identifier { get; }
  
    public int Count { get; set; }
  
    public int CurrentHp { get; set; }
  
    // public Vector2Int Position { get; set; }
  
    public bool IsDead { get; set; }
  
    public int CurrentActivityPoints { get; set; }

    public UnitState(UnitIdentifier identifier, int count, int currentHp, bool isDead, int currentActivityPoints)
    {
      Identifier = identifier;
      Count = count;
      CurrentHp = currentHp;
      // Position = position;
      IsDead = isDead;
      CurrentActivityPoints = currentActivityPoints;
    }
  }
}