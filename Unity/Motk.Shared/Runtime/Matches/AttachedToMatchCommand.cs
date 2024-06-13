using Unity.Netcode;

namespace Motk.Shared.Matches
{
  public struct AttachedToMatchCommand : INetworkSerializable
  {
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
    }
  }
}