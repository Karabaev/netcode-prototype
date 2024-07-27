namespace Motk.Client.Combat.Grid.Hex.Model
{
  public class CombatGridPayload
  {
    public readonly bool IsWalkable;

    public CombatGridPayload(bool isWalkable) => IsWalkable = isWalkable;
  }
}