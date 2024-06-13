using Unity.Collections;
using Unity.Netcode;

namespace Motk.Shared.Core.Net
{
  public class MessageSerializer
  {
    public FastBufferWriter Write<T>(T message) where T : INetworkSerializable
    {
      var writer = new FastBufferWriter(1024, Allocator.Temp);
      writer.WriteValueSafe(message);
      return writer;
    }
  }
}