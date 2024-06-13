// using System;
// using System.Collections.Generic;
// using System.Linq;
// using com.karabaev.utilities.unity;
// using Cysharp.Threading.Tasks;
// using JetBrains.Annotations;
// todokmo удалить
// namespace Motk.Matchmaking
// {
//   [UsedImplicitly]
//   public class MatchmakingService
//   {
//     private int _idCounter;
//
//     private readonly Dictionary<string, Room> _locationsToRooms = new(); // todokmo
//     private readonly Dictionary<Guid, Ticket> _allTickets = new(); // todokmo
//
//     private readonly Dictionary<int, List<Room>> _serversToRooms = new();
//     private readonly Dictionary<int, Room> _rooms = new();
//
//     public UniTask<Guid> CreateTicketAsync(string playerId, string locationId)
//     {
//       var ticketId = Guid.NewGuid();
//       var ticket = new Ticket(playerId, locationId);
//       _allTickets[ticketId] = ticket;
//       ticket.Status = TicketStatus.Found;
//       return UniTask.FromResult(ticketId);
//     }
//
//     public async UniTask<TicketStatusResponse> GetTicketStatusAsync(Guid ticketId)
//     {
//       await UniTask.Yield();
//       var ticket = _allTickets[ticketId];
//
//       ConnectionParameters? connectionParams = null;
//       if (ticket.Status == TicketStatus.Found)
//       {
//         connectionParams = new ConnectionParameters("127.0.0.1", 7777);
//       }
//
//       return new TicketStatusResponse(ticket.Status, connectionParams);
//     }
//
//     public UniTask DeleteRoomAsync(int roomId)
//     {
//       _rooms.Remove(roomId);
//       return UniTask.CompletedTask;
//     }
//
//     public UniTask<IReadOnlyList<int>> GetRoomsForServerAsync(int serverId)
//     {
//       var rooms = _serversToRooms[serverId];
//       return UniTask.FromResult<IReadOnlyList<int>>(rooms.Select(r => r.RoomId).ToList());
//     }
//
//     public UniTask<int> GetRoomIdForUser(string userSecret)
//     {
//       
//     }
//
//     public int FindRoom(ulong clientId, string locationId)
//     {
//       if (!_locationsToRooms.TryGetValue(locationId, out var room))
//       {
//         room = new Room(_idCounter);
//         _locationsToRooms.Add(locationId, room);
//         _idCounter++;
//       }
//       room.UserIds.Add(clientId);
//       return room.RoomId;
//     }
//
//     public void PlayerLeft(ulong clientId)
//     {
//       var locationRoomToRemove = string.Empty;
//       foreach (var (locationId, room) in _locationsToRooms)
//       {
//         if (!room.UserIds.Remove(clientId))
//           continue;
//
//         if (room.UserIds.Count <= 0)
//         {
//           locationRoomToRemove = locationId;
//         }
//         
//         break;
//       }
//
//       if (string.IsNullOrEmpty(locationRoomToRemove))
//       {
//         _locationsToRooms.Remove(locationRoomToRemove);
//       }
//     }
//   }
// }