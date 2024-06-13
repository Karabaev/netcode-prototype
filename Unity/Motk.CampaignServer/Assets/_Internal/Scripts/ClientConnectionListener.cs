using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Motk.CampaignServer.Locations;
using Motk.Matchmaking;
using Motk.Shared.Campaign.Actors;
using Motk.Shared.Campaign.Actors.States;
using Motk.Shared.Locations;
using Unity.Netcode;

namespace Motk.CampaignServer
{
  [UsedImplicitly]
  public class ClientConnectionListener : IDisposable
  {
    private readonly NetworkManager _networkManager;
    private readonly IPlayerLocationStorage _playerLocationStorage;
    private readonly MatchmakingService _matchmakingService;
    private readonly CampaignLocationsState _locationsState;

    private readonly Dictionary<ulong, int> _playerToRoom = new();
    
    public ClientConnectionListener(NetworkManager networkManager, IPlayerLocationStorage playerLocationStorage,
      MatchmakingService matchmakingService, CampaignLocationsState locationsState)
    {
      _networkManager = networkManager;
      _playerLocationStorage = playerLocationStorage;
      _matchmakingService = matchmakingService;
      _locationsState = locationsState;
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
      // отправлять запрос с клиента на присоединение игрока к матчу
      // от матчмейкинга у меня есть id юзера, id комнаты
      
      // создавать стейт актора для игрока
      
      
      // var playerLocationId = _playerLocationStorage.GetLocationId(clientId);
      // if (string.IsNullOrEmpty(playerLocationId))
      //   playerLocationId = "default";
      //
      // var roomId = _matchmakingService.FindRoom(clientId, playerLocationId);
      //
      // if (!_locationsState.RoomsToLocations.TryGet(roomId, out var locationState))
      // {
      //   locationState = new CampaignLocationState(playerLocationId);
      //   _locationsState.RoomsToLocations.Add(roomId, locationState);
      // }
      //
      // locationState.Actors.Add(clientId, new CampaignActorState());
      // _playerToRoom.Add(clientId, roomId);
    }

    private void OnClientDisconnected(ulong clientId)
    {
      _matchmakingService.PlayerLeft(clientId);
      _locationsState.RoomsToLocations.Remove(_playerToRoom[clientId]);
    }
  }
}