using com.karabaev.reactivetypes.Dictionary;
using Motk.Shared.Actors;

namespace Motk.Shared.Locations
{
  public class CampaignLocationState
  {
    public string LocationId { get; }
    
    public ReactiveDictionary<ulong, CampaignActorState> Actors { get; } = new();

    public CampaignLocationState(string locationId)
    {
      LocationId = locationId;
    }
  }
}