using Unity.Netcode;
using UnityEngine;

namespace Motk.Shared.Campaign.Actors.Messages
{
  public struct CampaignActorDto : INetworkSerializable
  {
    public ushort PlayerId;
    public Vector3 Position;
    public float EulerY;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref PlayerId);
      serializer.SerializeValue(ref Position);
      serializer.SerializeValue(ref EulerY);
    }
  }
}