using System.Collections.Generic;

namespace Motk.CampaignServer
{
  public interface IPlayerLocationStorage
  {
    void SetLocationId(ulong playerId, string locationId);
    
    string? GetLocationId(ulong playerId);
  }

  public class InMemoryPlayerLocationStorage : IPlayerLocationStorage
  {
    private readonly Dictionary<ulong, string> _playerToLocation = new();

    public void SetLocationId(ulong playerId, string locationId)
    {
      _playerToLocation[playerId] = locationId;
    }

    public string? GetLocationId(ulong playerId)
    {
      _playerToLocation.TryGetValue(playerId, out var locationId);
      return locationId;
    }
  }
}