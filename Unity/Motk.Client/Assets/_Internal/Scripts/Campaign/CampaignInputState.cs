using com.karabaev.reactivetypes.Action;
using UnityEngine;

namespace Game.Campaign
{
  public class CampaignInputState
  {
    public ReactiveAction<Vector3> GroundClicked { get; } = new();
  }
}