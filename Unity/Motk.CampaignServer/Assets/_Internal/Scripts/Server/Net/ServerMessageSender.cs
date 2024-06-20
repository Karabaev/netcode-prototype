using System.Collections.Generic;
using JetBrains.Annotations;
using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.CampaignServer.Server.Net
{
  [UsedImplicitly]
  public class ServerMessageSender
  {
    private readonly NetworkManager _networkManager;
    private readonly MessageSerializer _messageSerializer;
    
    public ServerMessageSender(NetworkManager networkManager, MessageSerializer messageSerializer)
    {
      _networkManager = networkManager;
      _messageSerializer = messageSerializer;
    }

    public void Broadcast<T>(T message) where T : INetworkSerializable
    {
      using var writer = _messageSerializer.Write(message);
      _networkManager.CustomMessagingManager.SendNamedMessageToAll(typeof(T).Name, writer);
    }

    public void Send<T>(T message, IReadOnlyList<ulong> receiverIds) where T : INetworkSerializable
    {
      using var writer = _messageSerializer.Write(message);
      _networkManager.CustomMessagingManager.SendNamedMessage(typeof(T).Name, receiverIds, writer);
    }
    
    public void Send<T>(T message, ulong receiverId) where T : INetworkSerializable
    {
      using var writer = _messageSerializer.Write(message);
      _networkManager.CustomMessagingManager.SendNamedMessage(typeof(T).Name, receiverId, writer);
    }
  }
}