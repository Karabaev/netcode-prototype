using com.karabaev.reactivetypes.Dictionary;
using JetBrains.Annotations;

namespace Motk.Shared.Campaign.Actors.States
{
  // todokmo на сервере используется CampaignLocationState, на клиенте CampaignActorsState. Надо привести к общему виду
  [UsedImplicitly]
  public class CampaignActorsState
  {
    public ReactiveDictionary<ulong, CampaignActorState> Actors { get; } = new();
  }
}