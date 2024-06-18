using Motk.Shared.Core.Net;

namespace Motk.CampaignServer.Match.Net
{
  public interface IMatchMessageReceiver
  {
    void OnMessageReceived<T>(ulong clientId, T message) where T : IMatchMessage, new();
  }
}