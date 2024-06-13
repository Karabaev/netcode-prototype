using com.karabaev.reactivetypes.Dictionary;
using JetBrains.Annotations;

namespace Motk.Shared.Campaign.Actors.States
{
  [UsedImplicitly]
  public class CampaignActorsState
  {
    public ReactiveDictionary<ulong, CampaignActorState> Actors { get; } = new();
  }
}