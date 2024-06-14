using Unity.Netcode;
using UnityEngine;

namespace Motk.Shared.Campaign.Movement.Messages
{
  public struct ActorMoveStartedCommand : INetworkSerializable
  {
    public ulong PlayerId;
    public Vector3[] Path;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref PlayerId);
      serializer.SerializeValue(ref Path);
    }
  }
}