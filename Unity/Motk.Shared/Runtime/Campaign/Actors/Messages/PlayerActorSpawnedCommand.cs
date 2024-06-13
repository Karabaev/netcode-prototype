using Unity.Netcode;

namespace Motk.Shared.Campaign.Actors.Messages
{
  public struct PlayerActorSpawnedCommand : INetworkSerializable
  {
    public int PlayerId;
    public CampaignActorDto Dto;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref PlayerId);
      serializer.SerializeValue(ref Dto);
    }
  }
}