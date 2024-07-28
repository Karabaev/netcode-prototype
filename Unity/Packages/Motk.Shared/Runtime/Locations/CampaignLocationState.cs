using com.karabaev.reactivetypes.Dictionary;
using Motk.Shared.Campaign.Actors.States;

namespace Motk.Shared.Locations
{
  public class CampaignLocationState
  {
    public ReactiveDictionary<ushort, CampaignActorState> Actors { get; } = new();
  }
}