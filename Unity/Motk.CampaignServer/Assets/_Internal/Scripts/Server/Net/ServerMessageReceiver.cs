using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Motk.CampaignServer.Server.States;
using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.CampaignServer.Server.Net
{
  [UsedImplicitly]
  public class ServerMessageReceiver
  {
    private readonly ServerState _serverState;
    private readonly NetworkManager _networkManager;
    private readonly Dictionary<int, Dictionary<string, List<Delegate>>> _matchMessageHandlers = new();

    public ServerMessageReceiver(ServerState serverState, NetworkManager networkManager)
    {
      _serverState = serverState;
      _networkManager = networkManager;
    }

    public void RegisterMessageHandler<T>(Action<ulong, T> action) where T : IServerMessage, new()
    {
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(typeof(T).Name, (clientId, reader) =>
      {
        reader.ReadValueSafe(out T message);
        action.Invoke(clientId, message);
      });
    }

    public void UnregisterMessageHandler<T>() where T : IServerMessage, new()
    {
      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(typeof(T).Name);
    }
    
    public void RegisterMatchMessageHandler<T>(int matchId, Action<ulong, T> action) where T : IMatchMessage, new()
    {
      if (!_matchMessageHandlers.TryGetValue(matchId, out var messageHandlers))
      {
        messageHandlers = new Dictionary<string, List<Delegate>>();
        _matchMessageHandlers.Add(matchId, messageHandlers);
      }
      
      var messageId = typeof(T).Name;
      if (messageHandlers.TryGetValue(messageId, out var handlers))
      {
        handlers.Add(action);
        return;
      }

      handlers = new List<Delegate> { action };
      messageHandlers.Add(messageId, handlers);

      RegisterMatchMessageHandler<T>(messageId);
    }

    public void UnregisterMatchMessageHandler<T>(int matchId) where T : IMatchMessage, new()
    {
      var messageId = typeof(T).Name;
      _matchMessageHandlers[matchId].Remove(messageId);
      if (_matchMessageHandlers[matchId].Count > 0)
        return;

      var hasMessageHandlers = _matchMessageHandlers.Any(mmh =>
      {
        if (mmh.Value.TryGetValue(messageId, out var handlers))
        {
          return handlers.Count > 0;
        }

        return false;
      });

      if (hasMessageHandlers)
        return;

      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(messageId);
    }

    private void RegisterMatchMessageHandler<T>(string messageId) where T : IMatchMessage, new()
    {
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(messageId, (clientId, reader) =>
      {
        var matchId = _serverState.ClientsInMatches[clientId];

        if (!_matchMessageHandlers.TryGetValue(matchId, out var messageHandlers))
          return;

        var handlers = messageHandlers[messageId];

        reader.ReadValueSafe(out T message);

        foreach (var handler in handlers)
          ((Action<ulong, T>)handler).Invoke(clientId, message);
      });
    }
  }
}