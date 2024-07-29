namespace Motk.Combat.Shared
{
  public struct CombatUnitIdentifierDto
  {
    public readonly ushort TeamId;
    public readonly ushort UnitId;

    public CombatUnitIdentifierDto(ushort teamId, ushort unitId)
    {
      TeamId = teamId;
      UnitId = unitId;
    }
  }
}