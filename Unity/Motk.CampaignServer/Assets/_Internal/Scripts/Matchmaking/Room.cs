using System.Collections.Generic;

namespace Motk.CampaignServer.Matchmaking
{
  public class Room
  {
    public int RoomId { get; }
      
    public HashSet<ulong> PlayerIds { get; }

    public Room(int roomId)
    {
      RoomId = roomId;
      PlayerIds = new HashSet<ulong>();
    }
  }
}