using System;
using System.Collections.Generic;
using com.karabaev.utilities;
using JetBrains.Annotations;
using Motk.CampaignServer.Match.Net;
using Motk.CampaignServer.Server.States;
using Motk.Shared.Core.Net;
using Unity.Netcode;

namespace Motk.CampaignServer.Server.Net
{
  [UsedImplicitly]
  public class ServerMessageReceiver
  {
    private readonly NetworkManager _networkManager;
    private readonly ServerState _serverState;
    private readonly Dictionary<int, IMatchMessageReceiver> _matchMessageHandlers = new();
    
    public ServerMessageReceiver(NetworkManager networkManager, ServerState serverState)
    {
      //
      _networkManager = networkManager;
      _serverState = serverState;
    }

    public void AddMatchHandler(int matchId, IMatchMessageReceiver messageHandler) => _matchMessageHandlers.Add(matchId, messageHandler);

    public void RemoveMatchHandler(int matchId) => _matchMessageHandlers.Remove(matchId);
    
    public void RegisterMessageHandler<T>(Action<ulong, T> handler) where T : INetworkSerializable, new()
    {
      if (!typeof(IMatchMessage).IsInterfaceImplemented<T>())
      {
        _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(typeof(T).Name, (clientId, payload) =>
        {
          payload.ReadValueSafe(out T message);
          handler.Invoke(clientId, message);
        });
        
        return;
      }
      
      _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(typeof(T).Name, HandleMatchMessage<T>);
    }

    private void HandleMatchMessage<T>(ulong clientId, FastBufferReader payload) where T : IMatchMessage, new()
    {
      payload.ReadValueSafe(out T message);

      var matchId = _serverState.ClientsInMatches[clientId];
      _matchMessageHandlers[matchId].OnMessageReceived(clientId, message);
    }
    
    public void UnregisterMessageHandler<T>() where T : INetworkSerializable, new()
    {
      _networkManager.CustomMessagingManager.UnregisterNamedMessageHandler(typeof(T).Name);
    }
  }
}