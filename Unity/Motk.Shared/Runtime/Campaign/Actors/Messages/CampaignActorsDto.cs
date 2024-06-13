using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Motk.Shared.Campaign.Actors.Messages
{
  public struct CampaignActorsDto : INetworkSerializable
  {
    public NativeList<CampaignActorDto> Actors;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Actors);
    }
  }

  public struct CampaignActorDto : INetworkSerializable
  {
    public ulong PlayerId;
    public Vector3 Position;
    public Quaternion Rotation;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref PlayerId);
      serializer.SerializeValue(ref Position);
      serializer.SerializeValue(ref Rotation);
    }
  }
}