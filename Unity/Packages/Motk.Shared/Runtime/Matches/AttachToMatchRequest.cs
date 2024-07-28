using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.Shared.Matches
{
  public struct AttachToMatchRequest : IServerMessage
  {
    public string UserSecret;
    public int MatchId;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref UserSecret);
      serializer.SerializeValue(ref MatchId);
    }
  }
}