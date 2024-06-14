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

    public PlayerPrefsUpdateContext<Dictionary<int, GameServerDescription>> GameServersRegistry => new("GameServersRegistry");

    public PlayerPrefsUpdateContext<Dictionary<int, Room>> RoomsRegistry => new("RoomsRegistry");
    
    public PlayerPrefsUpdateContext<Dictionary<Guid, Ticket>> TicketsRegistry => new("TicketsRegistry");
    
    public PlayerPrefsUpdateContext<Dictionary<string, string>> UserIdToUserSecret => new("UserIdToUserSecret");
    
    public PlayerPrefsUpdateContext<Dictionary<Guid, AllocationInfo>> TicketIdsToAllocations => new("TicketIdsToAllocations");
  }
}