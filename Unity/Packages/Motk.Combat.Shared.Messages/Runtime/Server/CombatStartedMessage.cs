using MessagePack;
using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Shared.Messages.Server
{
  [MessagePackObject]
  public readonly struct CombatStartedMessage
  {
    [Key(0)] public readonly ushort CurrentRoundIndex;
    [Key(1)] public readonly CombatUnitIdentifierDto[] TurnsQueue;
    [Key(2)] public readonly CombatTeamDto[] Teams;

    public CombatStartedMessage(ushort currentRoundIndex, CombatUnitIdentifierDto[] turnsQueue, CombatTeamDto[] teams)
    {
      CurrentRoundIndex = currentRoundIndex;
      TurnsQueue = turnsQueue;
      Teams = teams;
    }
  }
}