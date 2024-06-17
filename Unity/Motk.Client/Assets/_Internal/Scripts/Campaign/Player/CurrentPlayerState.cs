using JetBrains.Annotations;

namespace Motk.Client.Campaign.Player
{
  [UsedImplicitly]
  public class CurrentPlayerState
  {
    public string PlayerId { get; set; } = null!;
    
    public ulong ClientId { get; set; }
  }
}