using System.Collections.Generic;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.States;
using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.CampaignServer.Match.Net
{
  [UsedImplicitly]
  public class MatchMessageSender
  {
    private readonly NetworkManager _networkManager;
    private readonly MessageSerializer _messageSerializer;
    private readonly MatchState _matchState;
    
    public MatchMessageSender(NetworkManager networkManager, MessageSerializer messageSerializer, MatchState matchState)
    {
      _networkManager = networkManager;
      _messageSerializer = messageSerializer;
      _matchState = matchState;
    }

    public void Broadcast<T>(T message) where T : IMatchMessage
    {
      using var writer = _messageSerializer.Write(message);
      _networkManager.CustomMessagingManager.SendNamedMessage(typeof(T).Name, _matchState.ConnectedClients, writer);
    }
    
    public void Send<T>(T message, IReadOnlyList<ulong> receiverIds) where T : IMatchMessage
    {
      using var writer = _messageSerializer.Write(message);
      _networkManager.CustomMessagingManager.SendNamedMessage(typeof(T).Name, receiverIds, writer);
    }
    
    public void Send<T>(T message, ulong receiverId) where T : IMatchMessage
    {
      using var writer = _messageSerializer.Write(message);
      _networkManager.CustomMessagingManager.SendNamedMessage(typeof(T).Name, receiverId, writer);
    }
  }
}