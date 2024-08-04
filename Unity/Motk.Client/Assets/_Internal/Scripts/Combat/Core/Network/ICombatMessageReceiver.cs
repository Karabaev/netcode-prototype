using Motk.Client.Core;
using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Client.Core.Network
{
  public interface ICombatMessageReceiver
  {
    event NetworkMessageHandler<CombatTeamDto>? TeamJoined;
    event NetworkMessageHandler<ushort>? TeamLeft;
    event NetworkMessageHandler? UnitMoveAction;
    event NetworkMessageHandler? UnitWaitAction;
    event NetworkMessageHandler? UnitDefendAction;
    event NetworkMessageHandler? CombatStarted;
  }
}