using System;
using Unity.Netcode;

namespace Motk.Shared.Core.Net
{
  public class ServerMessageReceiver
  {
    private readonly NetworkManager _networkManager;

    public ServerMessageReceiver(NetworkManager networkManager) => _networkManager = networkManager;
    
    public void RegisterMessageHandler<T>(Action<ulong, T> handler) where T : INetworkSerializable, new()
    {
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(typeof(T).Name, (senderId, payload) =>
      {
        payload.ReadValueSafe(out T message);
        handler.Invoke(senderId, message);
      });
    }

    public void UnregisterMessageHandler<T>() where T : INetworkSerializable, new()
    {
      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(typeof(T).Name);
    }
  }
}