using System;
using System.Collections.Generic;
using System.Linq;
using com.karabaev.utilities.unity;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Motk.Matchmaking
{
  [UsedImplicitly]
  public class MatchmakingService
  {
    private readonly MatchmakingStorage _matchmakingStorage;
    
    public MatchmakingService(MatchmakingStorage matchmakingStorage)
    {
      _matchmakingStorage = matchmakingStorage;
      
      using var registry = _matchmakingStorage.GameServersRegistry;
      registry.Value.Clear();
      registry.Value.Add(1, new GameServerDescription(new ConnectionParameters("127.0.0.1", 7777)));
      registry.Value.Add(2, new GameServerDescription(new ConnectionParameters("127.0.0.2", 7777)));
      registry.Value.Add(3, new GameServerDescription(new ConnectionParameters("127.0.0.3", 7777)));
      
      using var registry2 = _matchmakingStorage.RoomsRegistry;
      registry2.Value.Clear();
      using var registry3 = _matchmakingStorage.TicketsRegistry;
      registry3.Value.Clear();
      using var registry4 = _matchmakingStorage.UserIdToUserSecret;
      registry4.Value.Clear();
      using var registry5 = _matchmakingStorage.TicketIdsToAllocations;
      registry5.Value.Clear();
    }
    
    public void Update()
    {
      using var ticketsRegistry = _matchmakingStorage.TicketsRegistry;
      foreach (var (ticketId, ticket) in ticketsRegistry.Value)
      {
        if (ticket.Status != TicketStatus.InProgress)
          continue;

        if (!TryFindRoom(ticket, out var room))
          continue;

        var newRoomId = _matchmakingStorage.TicketIdCounter++;
        room!.UserIds.Add(ticket.UserId);

        using var roomRegistry = _matchmakingStorage.RoomsRegistry;
        roomRegistry.Value[newRoomId] = room;
        ticket.Status = TicketStatus.Found;

        using var ticketAllocationRegistry = _matchmakingStorage.TicketIdsToAllocations;
        ticketAllocationRegistry.Value[ticketId] = new AllocationInfo(room.ServerId, newRoomId);
      }
    }

    public UniTask<Guid> CreateTicketAsync(string userId, string locationId)
    {
      var ticketId = Guid.NewGuid();
      var ticket = new Ticket(userId, locationId);
      using var ticketsRegistry = _matchmakingStorage.TicketsRegistry;
      ticketsRegistry.Value[ticketId] = ticket;

      using var userRegistry = _matchmakingStorage.UserIdToUserSecret; 
      userRegistry.Value[userId] = RandomUtils.RandomString();
      return UniTask.FromResult(ticketId);
    }

    public async UniTask<TicketStatusResponse> GetTicketStatusAsync(Guid ticketId)
    {
      await UniTask.Yield();
      using var registry = _matchmakingStorage.TicketsRegistry;
      var ticket = registry.Value[ticketId];

      ConnectionParameters? connectionParams = null;
      var roomId = -1;
      if (ticket.Status == TicketStatus.Found)
      {
        var allocation = _matchmakingStorage.TicketIdsToAllocations.Value[ticketId];
        connectionParams = _matchmakingStorage.GameServersRegistry.Value[allocation.ServerId].ConnectionParameters;
        roomId = allocation.RoomId;
      }

      var userSecret = _matchmakingStorage.UserIdToUserSecret.Value[ticket.UserId];
      
      return new TicketStatusResponse(userSecret, ticket.Status, connectionParams, roomId);
    }

    public UniTask DeleteRoomAsync(int roomId)
    {
      using var registry = _matchmakingStorage.RoomsRegistry;
      registry.Value.Remove(roomId);
      // todo удалить тикеты и освободить сервер
      return UniTask.CompletedTask;
    }

    public UniTask<int> GetRoomIdForUser(string userSecret)
    {
      var userId = _matchmakingStorage.UserIdToUserSecret.Value.First(u => u.Value == userSecret).Key;
      
      foreach (var (roomId, room) in _matchmakingStorage.RoomsRegistry.Value)
      {
        if (room.UserIds.Contains(userId))
        {
          return UniTask.FromResult(roomId);
        }
      }

      return UniTask.FromResult(-1);
    }

    public UniTask<string> GetLocationIdForRoom(int roomId)
    {
      return UniTask.FromResult(_matchmakingStorage.RoomsRegistry.Value[roomId].LocationId);
    }

    private bool TryFindRoom(Ticket ticket, out Room? result)
    {
      if (TryFindExistingRoom(ticket, out result))
        return true;

      return TryCreateRoom(ticket, out result);
    }
    
    private bool TryFindExistingRoom(Ticket ticket, out Room? result)
    {
      result = null;
      foreach (var (roomId, room) in _matchmakingStorage.RoomsRegistry.Value)
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
      foreach (var (serverId, _) in _matchmakingStorage.GameServersRegistry.Value)
      {
        result = new Room(ticket.LocationId, serverId);
        return true;
      }

      return false;
    }
  }
}