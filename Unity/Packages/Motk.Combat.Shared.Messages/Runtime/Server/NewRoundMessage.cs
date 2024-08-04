using MessagePack;
using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Shared.Messages.Server
{
  [MessagePackObject]
  public readonly struct NewRoundMessage
  {
    [Key(0)] public readonly ushort RoundIndex;
    [Key(1)] public readonly CombatUnitIdentifierDto[] TurnsQueue;

    public NewRoundMessage(ushort roundIndex, CombatUnitIdentifierDto[] turnsQueue)
    {
      RoundIndex = roundIndex;
      TurnsQueue = turnsQueue;
    }
  }
}