using System;
using System.Collections.Generic;
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
    private readonly Dictionary<int, MatchMessageHandlersBag> _matchMessageHandlers = new();
    private readonly Dictionary<string, Delegate> _serverMessageHandlers = new();

    public ServerMessageReceiver(ServerState serverState, NetworkManager networkManager)
    {
      _serverState = serverState;
      _networkManager = networkManager;
    }

    public void RegisterMessageHandler<T>(Action<ushort, T> action) where T : IServerMessage, new()
    {
      var messageId = typeof(T).Name;
      _serverMessageHandlers.Add(messageId, action);
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(typeof(T).Name, OnSeverMessageReceived<T>);
    }
    
    public void UnregisterMessageHandler<T>() where T : IServerMessage, new()
    {
      var messageId = typeof(T).Name;
      _serverMessageHandlers.Remove(messageId);
      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(messageId);
    }
    
    public void RegisterMatchMessageHandler<T>(int matchId, Action<ushort, T> action) where T : IMatchMessage, new()
    {
      if (!_matchMessageHandlers.TryGetValue(matchId, out var messageHandlers))
      {
        messageHandlers = new MatchMessageHandlersBag();
        _matchMessageHandlers.Add(matchId, messageHandlers);
      }
      
      var messageId = typeof(T).Name;
      messageHandlers.Handlers.Add(messageId, action);
      
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(messageId, OnMatchMessageReceived<T>);
    }

    public void UnregisterMatchMessageHandler<T>(int matchId) where T : IMatchMessage, new()
    {
      var messageId = typeof(T).Name;

      var matchMessageHandlers = _matchMessageHandlers[matchId];
      matchMessageHandlers.Handlers.Remove(messageId);

      var hasMessageHandlers = false;
      foreach (var (_, handlers) in _matchMessageHandlers)
      {
        if (handlers.Handlers.ContainsKey(messageId))
        {
          hasMessageHandlers = true;
          break;
        }
      }
      
      if (hasMessageHandlers)
        return;

      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(messageId);
    }
    
    private void OnSeverMessageReceived<T>(ulong clientId, FastBufferReader reader) where T : IServerMessage, new()
    {
      var messageId = typeof(T).Name;

      reader.ReadValueSafe(out T message);
      var handler = _serverMessageHandlers[messageId];
      ((Action<ushort, T>)handler).Invoke((ushort) clientId, message);
    }
    
    private void OnMatchMessageReceived<T>(ulong clientId, FastBufferReader reader)  where T : IMatchMessage, new()
    {
      var matchId = _serverState.ClientsInMatches[clientId];

      if (!_matchMessageHandlers.TryGetValue(matchId, out var messageHandlers))
        return;

      reader.ReadValueSafe(out T message);

      var messageId = typeof(T).Name;
      var handler = messageHandlers.Handlers[messageId];
      ((Action<ushort, T>)handler).Invoke((ushort)clientId, message);
    }
    
    private class MatchMessageHandlersBag
    {
      public Dictionary<string, Delegate> Handlers { get; } = new();
    }
  }
}