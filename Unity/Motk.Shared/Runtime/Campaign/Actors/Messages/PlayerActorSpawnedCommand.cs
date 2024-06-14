using Unity.Netcode;

namespace Motk.Shared.Campaign.Actors.Messages
{
  public struct PlayerActorSpawnedCommand : INetworkSerializable
  {
    public CampaignActorDto Actor;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Actor);
    }
  }
}