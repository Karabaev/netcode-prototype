using com.karabaev.reactivetypes.Action;
using JetBrains.Annotations;
using UnityEngine;

namespace Motk.Client.Campaign.InputSystem
{
  [UsedImplicitly]
  public class CampaignInputState
  {
    public ReactiveAction<Vector3> GroundClicked { get; } = new();
  }
}