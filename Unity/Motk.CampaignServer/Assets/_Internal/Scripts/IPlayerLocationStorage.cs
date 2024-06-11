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
    private readonly Dictionary<ulong, string> _playerTtoLocation = new();

    public void SetLocationId(ulong playerId, string locationId)
    {
      _playerTtoLocation[playerId] = locationId;
    }

    public string? GetLocationId(ulong playerId)
    {
      _playerTtoLocation.TryGetValue(playerId, out var locationId);
      return locationId;
    }
  }
}