using com.karabaev.reactivetypes.Dictionary;
using JetBrains.Annotations;
using Motk.Shared.Locations;

namespace Motk.CampaignServer.Locations
{
  [UsedImplicitly]
  public class CampaignLocationsState
  {
    public ReactiveDictionary<int, CampaignLocationState> RoomsToLocations { get; } = new();
  }
}