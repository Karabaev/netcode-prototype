namespace Motk.CombatServer.Core;

public readonly struct UnitIdentifier : IEquatable<UnitIdentifier>
{
  public int TeamId { get; }
  
  public int UnitId { get; }

  public UnitIdentifier(int teamId, int unitId)
  {
    TeamId = teamId;
    UnitId = unitId;
  }

  public bool Equals(UnitIdentifier other) => TeamId == other.TeamId && UnitId == other.UnitId;

  public override bool Equals(object? obj) => obj is UnitIdentifier other && Equals(other);

  public override int GetHashCode() => HashCode.Combine(TeamId, UnitId);
}