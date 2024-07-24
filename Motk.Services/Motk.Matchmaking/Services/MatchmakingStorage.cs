using System.Collections.Concurrent;
using Motk.Matchmaking.Models;

namespace Motk.Matchmaking.Services;

public class MatchmakingStorage
{
    public int RoomIdCounter { get; set; }

    public Dictionary<int, GameServerDescription> GameServersRegistry { get; } = new();

    public Dictionary<int, Room> RoomsRegistry { get; } = new();

    public ConcurrentDictionary<Guid, Ticket> TicketsRegistry { get; } = new();

    public Dictionary<string, string> UserIdToUserSecret { get; } = new();

    public ConcurrentDictionary<Guid, AllocationInfo> TicketIdsToAllocations { get; } = new();
}