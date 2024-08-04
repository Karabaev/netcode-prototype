using Motk.Combat.Shared.Messages.Dto;

namespace Motk.Combat.Shared.gRPC.Services
{
  public interface ICombatHubReceiver
  {
    void OnTeamJoined(CombatTeamDto team);
    void OnTeamLeft(ushort teamId);
    void OnCombatStarted();
    void OnUnitMoveAction();
    void OnUnitWaitAction();
    void OnUnitDefendAction();
  }
}