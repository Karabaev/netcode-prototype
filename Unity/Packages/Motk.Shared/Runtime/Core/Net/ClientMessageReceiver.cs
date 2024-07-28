using System;
using Unity.Netcode;

namespace Motk.Shared.Core.Net
{
  public class ClientMessageReceiver
  {
    private readonly NetworkManager _networkManager;

    public ClientMessageReceiver(NetworkManager networkManager) => _networkManager = networkManager;
    
    public void RegisterMessageHandler<T>(Action<T> handler) where T : INetworkSerializable, new()
    {
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(typeof(T).Name, (_, payload) =>
      {
        payload.ReadValueSafe(out T message);
        handler.Invoke(message);
      });
    }

    public void UnregisterMessageHandler<T>() where T : INetworkSerializable, new()
    {
      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(typeof(T).Name);
    }
  }
}