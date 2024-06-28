namespace Motk.Matchmaking.New;

public class MatchmakingStorage
{
    public int TicketIdCounter { get; set; }

    public Dictionary<int, GameServerDescription> GameServersRegistry { get; } = new();

    public Dictionary<int, Room> RoomsRegistry { get; } = new();

    public Dictionary<Guid, Ticket> TicketsRegistry { get; } = new();

    public Dictionary<string, string> UserIdToUserSecret { get; } = new();

    public Dictionary<Guid, AllocationInfo> TicketIdsToAllocations { get; } = new();
}