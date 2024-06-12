using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Motk.Matchmaking
{
  [UsedImplicitly]
  public class MatchmakingService
  {
    private int _idCounter;

    private readonly Dictionary<string, Room> _locationsToRooms = new();

    private readonly Dictionary<Guid, Ticket> _allTickets = new();

    public UniTask<Guid> CreateTicketAsync(string playerId, string locationId)
    {
      var ticketId = Guid.NewGuid();
      var ticket = new Ticket(playerId, locationId);
      _allTickets[ticketId] = ticket;
      ticket.Status = TicketStatus.Found;
      return UniTask.FromResult(ticketId);
    }

    public async UniTask<TicketStatusResponse> GetTicketStatusAsync(Guid ticketId)
    {
      await UniTask.Yield();
      var ticket = _allTickets[ticketId];

      ConnectionParameters? connectionParams = null;
      if (ticket.Status == TicketStatus.Found)
      {
        connectionParams = new ConnectionParameters("127.0.0.1", 7777);
      }
      
      return new TicketStatusResponse(ticket.Status, connectionParams);
    }

    public int FindRoom(ulong clientId, string locationId)
    {
      if (!_locationsToRooms.TryGetValue(locationId, out var room))
      {
        room = new Room(_idCounter);
        _locationsToRooms.Add(locationId, room);
        _idCounter++;
      }
      room.PlayerIds.Add(clientId);
      return room.RoomId;
    }

    public void PlayerLeft(ulong clientId)
    {
      var locationRoomToRemove = string.Empty;
      foreach (var (locationId, room) in _locationsToRooms)
      {
        if (!room.PlayerIds.Remove(clientId))
          continue;

        if (room.PlayerIds.Count <= 0)
        {
          locationRoomToRemove = locationId;
        }
        
        break;
      }

      if (string.IsNullOrEmpty(locationRoomToRemove))
      {
        _locationsToRooms.Remove(locationRoomToRemove);
      }
    }
  }
}