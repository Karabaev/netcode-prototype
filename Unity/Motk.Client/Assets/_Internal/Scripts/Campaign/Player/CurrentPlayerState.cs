using JetBrains.Annotations;

namespace Motk.Campaign.Client.Player
{
  [UsedImplicitly]
  public class CurrentPlayerState
  {
    public string PlayerId { get; set; } = null!;
    
    public ulong ClientId { get; set; }
  }
}