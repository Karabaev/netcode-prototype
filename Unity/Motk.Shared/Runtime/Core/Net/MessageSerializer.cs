using JetBrains.Annotations;
using Unity.Collections;
using Unity.Netcode;

namespace Motk.Shared.Core.Net
{
  [UsedImplicitly]
  public class MessageSerializer
  {
    public FastBufferWriter Write<T>(T message) where T : INetworkSerializable
    {
      var writer = new FastBufferWriter(128, Allocator.Temp, 1024);
      writer.WriteValueSafe(message);
      return writer;
    }
  }
}