using Unity.Netcode;
using UnityEngine;

namespace Motk.Shared.Campaign.Movement
{
  public struct FindPathRequest : INetworkSerializable
  {
    public Vector3 Destination;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Destination);
    }
  }
}