using Motk.Combat.Shared;

namespace Motk.Client.Combat.Network.Server
{
  public readonly struct CombatRoundStartedCommand
  {
    public readonly ushort Index;
    public readonly CombatUnitIdentifierDto[] TurnsQueue;
  }
}