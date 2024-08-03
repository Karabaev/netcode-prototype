using Motk.Combat.Shared;

namespace Motk.Combat.Client.Core.Network.Server
{
  public readonly struct CombatRoundStartedCommand
  {
    public readonly ushort Index;
    public readonly CombatUnitIdentifierDto[] TurnsQueue;
  }
}