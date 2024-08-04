using MessagePack;
using Motk.HexGrid.Core;

namespace Motk.Combat.Shared.Messages.Dto
{
  [MessagePackObject]
  public readonly struct CombatUnitDto
  {
    [Key(0)] public readonly string DescriptorId;
    [Key(1)] public readonly byte Id;
    [Key(2)] public readonly HexCoordinatesDto Position;
    [Key(3)] public readonly HexDirection Direction;
    [Key(4)] public readonly int Count;
    [Key(5)] public readonly float CurrentHp;

    public CombatUnitDto(string descriptorId, byte id, HexCoordinatesDto position, HexDirection direction, int count, float currentHp)
    {
      DescriptorId = descriptorId;
      Id = id;
      Position = position;
      Direction = direction;
      Count = count;
      CurrentHp = currentHp;
    }
  }
}