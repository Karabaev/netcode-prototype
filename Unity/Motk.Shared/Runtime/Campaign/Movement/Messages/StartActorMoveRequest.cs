using Unity.Netcode;
using UnityEngine;

namespace Motk.Shared.Campaign.Movement.Messages
{
  public struct StartActorMoveRequest : INetworkSerializable
  {
    public Vector3 Destination;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Destination);
    }
  }
}