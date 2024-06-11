using System.Collections.Generic;
using JetBrains.Annotations;

namespace Motk.CampaignServer.Matchmaking
{
  [UsedImplicitly]
  public class MatchmakingService
  {
    private int _idCounter;
    
    private readonly Dictionary<string, Room> _locationsToRooms = new();

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
        if (room.PlayerIds.Remove(clientId))
        {
          locationRoomToRemove = locationId;
        }

      }

      if (string.IsNullOrEmpty(locationRoomToRemove))
      {
        _locationsToRooms.Remove(locationRoomToRemove);
      }
    }
  }
}