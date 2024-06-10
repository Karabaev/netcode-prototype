using com.karabaev.reactivetypes.Action;
using UnityEngine;

namespace Motk.Client.Campaign
{
  public class CampaignInputState
  {
    public ReactiveAction<Vector3> GroundClicked { get; } = new();
  }
}