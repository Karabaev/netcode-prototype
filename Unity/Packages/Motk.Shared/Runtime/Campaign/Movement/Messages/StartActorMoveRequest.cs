using Motk.Shared.Core.Net;
using Unity.Netcode;
using UnityEngine;

namespace Motk.Shared.Campaign.Movement.Messages
{
  public struct StartActorMoveRequest : IMatchMessage
  {
    public Vector3 Destination;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Destination);
    }
  }
}