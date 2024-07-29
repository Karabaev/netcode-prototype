using System;

namespace Motk.Combat.Shared
{
  public readonly struct CombatUnitIdentifier : IEquatable<CombatUnitIdentifier>
  {
    public readonly ushort TeamId;
    public readonly ushort UnitId;

    public CombatUnitIdentifier(ushort teamId, ushort unitId)
    {
      TeamId = teamId;
      UnitId = unitId;
    }

    public bool Equals(CombatUnitIdentifier other) => TeamId == other.TeamId && UnitId == other.UnitId;

    public override bool Equals(object? obj) => obj is CombatUnitIdentifier other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(TeamId, UnitId);
  }
}