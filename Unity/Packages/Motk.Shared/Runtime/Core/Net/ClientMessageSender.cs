using Unity.Netcode;

namespace Motk.Shared.Core.Net
{
  public class ClientMessageSender
  {
    private readonly NetworkManager _networkManager;
    private readonly MessageSerializer _serializer;

    public ClientMessageSender(NetworkManager networkManager, MessageSerializer serializer)
    {
      _networkManager = networkManager;
      _serializer = serializer;
    }

    public void Send<T>(T message) where T : INetworkSerializable
    {
      using var writer = _serializer.Write(message);
      _networkManager.CustomMessagingManager.SendNamedMessage(typeof(T).Name, NetworkManager.ServerClientId, writer);
    }
  }
}