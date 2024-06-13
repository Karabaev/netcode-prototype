using Unity.Netcode;

namespace Motk.Shared.Campaign.Actors.Messages
{
  public struct CampaignActorsStateMessage : INetworkSerializable
  {
    public CampaignActorsDto Actors;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Actors);
    }
  }
}