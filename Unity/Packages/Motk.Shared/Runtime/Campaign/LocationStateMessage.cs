using Motk.Shared.Campaign.Actors.Messages;
using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.Shared.Campaign
{
  public struct LocationStateMessage : IMatchMessage
  {
    public CampaignActorDto[] Actors;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Actors);
    }
  }
}