using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.Shared.Campaign.Actors.Messages
{
  public struct PlayerActorDespawnedCommand : IMatchMessage
  {
    public ulong PlayerId;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref PlayerId);
    }
  }
}