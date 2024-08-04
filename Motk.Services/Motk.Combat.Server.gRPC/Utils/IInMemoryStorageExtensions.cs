using MagicOnion.Server.Hubs;

namespace Motk.Combat.Server.gRPC.Utils;

public static class IInMemoryStorageExtensions
{
  public static T Require<T>(this IInMemoryStorage<T> storage, Guid connectionId) where T : class
  {
    var data = storage.Get(connectionId);
    if (data == null)
      throw new KeyNotFoundException($"Record not found in storage for connection. ConnectionId={connectionId}");

    return data;
  }
}