using System;
using Unity.Netcode;

namespace Motk.Shared.Core.Net
{
  // todokmo отрефачить на клиентский и серверный
  public abstract class MessageReceiver<T> : IDisposable where T : INetworkSerializable, new()
  {
    private readonly NetworkManager _networkManager;

    public MessageReceiver(NetworkManager networkManager)
    {
      _networkManager = networkManager;
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(typeof(T).Name, OnMessageReceived);
    }

    public void Dispose()
    {
      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(typeof(T).Name);
    }

    private void OnMessageReceived(ulong senderId, FastBufferReader payload)
    {
      payload.ReadValueSafe(out T message);
      OnMessageReceived(senderId, message);
    }

    protected abstract void OnMessageReceived(ulong senderId, T message);
  }
}