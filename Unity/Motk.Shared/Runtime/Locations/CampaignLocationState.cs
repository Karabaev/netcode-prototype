using com.karabaev.reactivetypes.Dictionary;
using Motk.Shared.Campaign.Actors;
using Motk.Shared.Campaign.Actors.States;

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