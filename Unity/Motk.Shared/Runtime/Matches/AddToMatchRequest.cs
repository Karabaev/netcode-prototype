using Unity.Netcode;

namespace Motk.Shared.Matches
{
  public class AddToMatchRequest : INetworkSerializable
  {
    public int UserId;
    public int MatchId;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref UserId);
      serializer.SerializeValue(ref MatchId);
    }
  }
}