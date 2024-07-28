using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.Shared.Campaign.Actors.Messages
{
  public struct PlayerActorSpawnedCommand : IMatchMessage
  {
    public CampaignActorDto Actor;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Actor);
    }
  }
}