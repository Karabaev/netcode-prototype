using Motk.HexGrid.Core.Descriptors;

namespace Motk.Combat.Shared
{
  public readonly struct CombatUnitDto
  {
    public readonly string DescriptorId;
    public readonly HexCoordinates Position;
    public readonly int Count;
    public readonly float CurrentHp;

    public CombatUnitDto(string descriptorId, HexCoordinates position, int count, float currentHp)
    {
      DescriptorId = descriptorId;
      Position = position;
      Count = count;
      CurrentHp = currentHp;
    }
  }
}