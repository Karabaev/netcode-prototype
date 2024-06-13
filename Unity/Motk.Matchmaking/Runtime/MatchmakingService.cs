using System;
using System.Collections.Generic;
using System.Linq;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;

namespace Motk.Matchmaking
{
  public class MatchmakingService
  {
    private int _idCounter;

    private readonly Dictionary<int, GameServerDescription> _gameServersRegistry = new();
    
    private readonly Dictionary<int, Room> _roomsRegistry = new();
    private readonly Dictionary<Guid, Ticket> _ticketsRegistry = new();
    
    private readonly Dictionary<string, string> _userIdToUserSecret = new();
    private readonly Dictionary<Guid, AllocationInfo> _ticketIdsToAllocations = new();

    public MatchmakingService()
    {
      _gameServersRegistry.Add(1, new GameServerDescription(new ConnectionParameters("127.0.0.1", 7777)));
      _gameServersRegistry.Add(2, new GameServerDescription(new ConnectionParameters("127.0.0.2", 7777)));
      _gameServersRegistry.Add(3, new GameServerDescription(new ConnectionParameters("127.0.0.3", 7777)));
    }
    
    public void Update()
    {
      foreach (var (ticketId, ticket) in _ticketsRegistry)
      {
        if (ticket.Status != TicketStatus.InProgress)
          continue;

        if (!TryFindRoom(ticket, out var room))
          continue;

        var newRoomId = _idCounter++;
        _roomsRegistry[newRoomId] = room!;
        room!.UserIds.Add(ticket.UserId);
        ticket.Status = TicketStatus.Found;
        _ticketIdsToAllocations[ticketId] = new AllocationInfo(room!.ServerId, newRoomId);
      }
    }

    public UniTask<Guid> CreateTicketAsync(string userId, string locationId)
    {
      var ticketId = Guid.NewGuid();
      var ticket = new Ticket(userId, locationId);
      _ticketsRegistry[ticketId] = ticket;
      
      _userIdToUserSecret[userId] = RandomUtils.RandomString();
      return UniTask.FromResult(ticketId);
    }

    public async UniTask<TicketStatusResponse> GetTicketStatusAsync(Guid ticketId)
    {
      await UniTask.Yield();
      var ticket = _ticketsRegistry[ticketId];

      ConnectionParameters? connectionParams = null;
      var roomId = -1;
      if (ticket.Status == TicketStatus.Found)
      {
        var allocation = _ticketIdsToAllocations[ticketId];
        connectionParams = _gameServersRegistry[allocation.ServerId].ConnectionParameters;
        roomId = allocation.RoomId;
      }

      var userSecret = _userIdToUserSecret[ticket.UserId];
      
      return new TicketStatusResponse(userSecret, ticket.Status, connectionParams, roomId);
    }

    public UniTask DeleteRoomAsync(int roomId)
    {
      _roomsRegistry.Remove(roomId);
      // todo удалить тикеты и освободить сервер
      return UniTask.CompletedTask;
    }

    public UniTask<int> GetRoomIdForUser(string userSecret)
    {
      var userId = _userIdToUserSecret.First(u => u.Value == userSecret).Key;
      
      foreach (var (roomId, room) in _roomsRegistry)
      {
        if (room.UserIds.Contains(userId))
        {
          return UniTask.FromResult(roomId);
        }
      }

      return UniTask.FromResult(-1);
    }

    public UniTask<string> GetLocationIdForRoom(int roomId) => UniTask.FromResult(_roomsRegistry[roomId].LocationId);

    private bool TryFindRoom(Ticket ticket, out Room? result)
    {
      if (TryFindExistingRoom(ticket, out result))
        return true;

      return TryCreateRoom(ticket, out result);
    }
    
    private bool TryFindExistingRoom(Ticket ticket, out Room? result)
    {
      result = null;
      foreach (var (roomId, room) in _roomsRegistry)
      {
        if (ticket.LocationId == room.LocationId)
        {
          result = room;
          return true;
        }
      }

      return false;
    }

    private bool TryCreateRoom(Ticket ticket, out Room? result)
    {
      result = null;
      foreach (var (serverId, _) in _gameServersRegistry)
      {
        result = new Room(ticket.LocationId, serverId);
        return true;
      }

      return false;
    }
  }
}