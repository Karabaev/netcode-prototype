using com.karabaev.reactivetypes.Action;
using JetBrains.Annotations;
using UnityEngine;

namespace Motk.Campaign.Client.InputSystem
{
  [UsedImplicitly]
  public class CampaignInputState
  {
    public ReactiveAction<Vector3> GroundClicked { get; } = new();
  }
}