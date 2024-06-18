using System;
using System.Linq;
using JetBrains.Annotations;
using Motk.CampaignServer.Server.States;
using Unity.Netcode;
using UnityEngine;

namespace Motk.CampaignServer.Server
{
  [UsedImplicitly]
  public class ClientConnectionListener : IDisposable
  {
    private readonly NetworkManager _networkManager;
    private readonly ServerState _serverState;
    
    public ClientConnectionListener(NetworkManager networkManager, ServerState serverState)
    {
      _networkManager = networkManager;
      _serverState = serverState;
      _networkManager.OnClientConnectedCallback += OnClientConnected;
      _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public void Dispose()
    {
      _networkManager.OnClientConnectedCallback -= OnClientConnected;
      _networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    private void OnClientConnected(ulong clientId)
    {
      Debug.Log($"Client connected. ClientId={clientId}");
    }

    private void OnClientDisconnected(ulong clientId)
    {
      Debug.Log($"Client disconnected. ClientId={clientId}");

      foreach (var (_, matchState) in _serverState.Matches.ToList())
      {
        foreach (var (userId, userClientId) in matchState.Users.ToList())
        {
          if (userClientId == clientId)
            matchState.Users.Remove(userId);
        }
      }
      _serverState.ClientsInMatches.Remove(clientId);

    }
  }
}