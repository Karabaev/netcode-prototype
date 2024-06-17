using System;
using System.Linq;
using JetBrains.Annotations;
using Motk.CampaignServer.Matches.States;
using Motk.Matchmaking;
using Unity.Netcode;

namespace Motk.CampaignServer
{
  [UsedImplicitly]
  public class ClientConnectionListener : IDisposable
  {
    private readonly NetworkManager _networkManager;
    private readonly MatchesState _matchesState;
    
    public ClientConnectionListener(NetworkManager networkManager, MatchesState matchesState)
    {
      _networkManager = networkManager;
      _matchesState = matchesState;
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
    }

    private void OnClientDisconnected(ulong clientId)
    {
      foreach (var (_, matchState) in _matchesState.Matches.ToList())
      {
        foreach (var (userId, userClientId) in matchState.Users.ToList())
        {
          if (userClientId == clientId)
            matchState.Users.Remove(userId);
        }
      }
    }
  }
}