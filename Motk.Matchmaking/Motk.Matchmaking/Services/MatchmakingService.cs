using Motk.Matchmaking.Dtos;
using Motk.Matchmaking.Models;
using Motk.Matchmaking.Utils;

namespace Motk.Matchmaking.Services
{
  // Вместо уведомления матчмейкингом об аллокации комнаты на сервере игровой сервер сам запрашивает инфу о комнате с матчмейкинга.
  // Таким образом, когда клиент начнет подключение, игровой сервер сможет получить нужную инфу.
  // В обратном случае, матчмейкинг может не успеть достучаться до игрового сервера к моменту, когда клиент начнет подключение.
  public class MatchmakingService
  {
    private readonly MatchmakingStorage _matchmakingStorage;
    private readonly Random _random;
    
    public MatchmakingService(MatchmakingStorage matchmakingStorage)
    {
      _random = new Random();
      _matchmakingStorage = matchmakingStorage;

      var registry = _matchmakingStorage.GameServersRegistry;
      registry.Add(1, new GameServerDescription(new ConnectionParameters("127.0.0.1", 7777)));
      registry.Add(2, new GameServerDescription(new ConnectionParameters("127.0.0.2", 7777)));
      registry.Add(3, new GameServerDescription(new ConnectionParameters("127.0.0.3", 7777)));
    }
    
    public void Update()
    {
      foreach (var (ticketId, ticket) in _matchmakingStorage.TicketsRegistry)
      {
        if (ticket.Status != TicketStatus.InProgress)
          continue;

        if (!TryFindRoomId(ticket, out var roomId))
          continue;

        var room = _matchmakingStorage.RoomsRegistry[roomId];
        room.UserIds.Add(ticket.UserId);
        ticket.Status = TicketStatus.Found;

        _matchmakingStorage.TicketIdsToAllocations[ticketId] = new AllocationInfo(room.ServerId, roomId);
      }
    }

    public Task<Guid> CreateTicketAsync(string userId, string locationId)
    {
      var ticketId = Guid.NewGuid();
      var ticket = new Ticket(userId, locationId);
      _matchmakingStorage.TicketsRegistry[ticketId] = ticket;

      if (!_matchmakingStorage.UserIdToUserSecret.ContainsKey(userId))
        _matchmakingStorage.UserIdToUserSecret[userId] = RandomUtils.RandomString(_random);
      
      return Task.FromResult(ticketId);
    }

    public async Task<TicketStatusResponse> GetTicketStatusAsync(Guid ticketId)
    {
      await Task.Yield();
      var ticket = _matchmakingStorage.TicketsRegistry[ticketId];

      ConnectionParameters? connectionParams = null;
      var roomId = -1;
      if (ticket.Status == TicketStatus.Found)
      {
        var allocation = _matchmakingStorage.TicketIdsToAllocations[ticketId];
        connectionParams = _matchmakingStorage.GameServersRegistry[allocation.ServerId].ConnectionParameters;
        roomId = allocation.RoomId;
      }

      var userSecret = _matchmakingStorage.UserIdToUserSecret[ticket.UserId];
      
      return new TicketStatusResponse(userSecret, ticket.Status, connectionParams, roomId);
    }

    public Task<int> GetRoomIdForUserAsync(string userSecret)
    {
      var userId = _matchmakingStorage.UserIdToUserSecret.First(u => u.Value == userSecret).Key;
      
      foreach (var (roomId, room) in _matchmakingStorage.RoomsRegistry)
      {
        if (room.UserIds.Contains(userId))
        {
          return Task.FromResult(roomId);
        }
      }

      return Task.FromResult(-1);
    }

    public Task<string> GetLocationIdForRoomAsync(int roomId)
    {
      return Task.FromResult(_matchmakingStorage.RoomsRegistry[roomId].LocationId);
    }

    public async Task RemoveUserFromRoomAsync(string userSecret)
    {
      var roomId = await GetRoomIdForUserAsync(userSecret);
      
      var userId = _matchmakingStorage.UserIdToUserSecret.First(u => u.Value == userSecret).Key;
      var room = _matchmakingStorage.RoomsRegistry[roomId];
      room.UserIds.Remove(userId);

      if (room.UserIds.Count > 0) return;

      _matchmakingStorage.RoomsRegistry.Remove(roomId);
      // todokmo удалить тикеты и освободить сервер
    }

    private bool TryFindRoomId(Ticket ticket, out int result)
    {
      if (TryFindExistingRoomId(ticket, out result))
        return true;

      return TryCreateRoom(ticket, out result);
    }
    
    private bool TryFindExistingRoomId(Ticket ticket, out int result)
    {
      result = -1;
      foreach (var (roomId, room) in _matchmakingStorage.RoomsRegistry)
      {
        if (room.LocationId == ticket.LocationId)
        {
          result = roomId;
          return true;
        }
      }

      return false;
    }

    private bool TryCreateRoom(Ticket ticket, out int newRoomId)
    {
      newRoomId = -1;
      
      foreach (var (serverId, _) in _matchmakingStorage.GameServersRegistry)
      {
        // todokmo добавить проверку на вместимость сервера
        newRoomId = _matchmakingStorage.TicketIdCounter++;
        var newRoom = new Room(ticket.LocationId, serverId);
        _matchmakingStorage.RoomsRegistry[newRoomId] = newRoom;
        return true;
      }

      return false;
    }
  }
}