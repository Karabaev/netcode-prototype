using System.Collections.Generic;
using Motk.Combat.Shared;

namespace Motk.Combat.Client.Core.Network.Server
{
  public readonly struct CombatStateMessage
  {
    public readonly ushort RoundIndex;
    public readonly CombatUnitIdentifierDto[] TurnsQueue;
    public readonly Dictionary<ushort, CombatTeamDto> Teams;

    public CombatStateMessage(ushort roundIndex, CombatUnitIdentifierDto[] turnsQueue, Dictionary<ushort, CombatTeamDto> teams)
    {
      RoundIndex = roundIndex;
      TurnsQueue = turnsQueue;
      Teams = teams;
    }
  }
}