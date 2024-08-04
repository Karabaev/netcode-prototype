namespace Motk.Combat.Server.gRPC;

public class CombatPlayer
{
  public readonly string UserSecret;
  public readonly byte TeamId;
  public bool IsReady { get; set; }

  public CombatPlayer(string userSecret, byte teamId)
  {
    UserSecret = userSecret;
    TeamId = teamId;
  }
}