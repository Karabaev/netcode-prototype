using System;
using System.Collections.Generic;
using UnityEngine;

namespace Motk.Matchmaking
{
  public class MatchmakingStorage
  {
    public int TicketIdCounter
    {
      get => PlayerPrefs.GetInt(nameof(TicketIdCounter));
      set => PlayerPrefs.SetInt(nameof(TicketIdCounter), value);
    }

    public UpdateContext<Dictionary<int, GameServerDescription>> GameServersRegistry => new("GameServersRegistry");

    public UpdateContext<Dictionary<int, Room>> RoomsRegistry => new("RoomsRegistry");
    
    public UpdateContext<Dictionary<Guid, Ticket>> TicketsRegistry => new("TicketsRegistry");
    
    public UpdateContext<Dictionary<string, string>> UserIdToUserSecret => new("UserIdToUserSecret");
    
    public UpdateContext<Dictionary<Guid, AllocationInfo>> TicketIdsToAllocations => new("TicketIdsToAllocations");
  }
}