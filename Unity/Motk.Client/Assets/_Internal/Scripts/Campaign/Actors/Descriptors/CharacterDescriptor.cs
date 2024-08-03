using System;
using Motk.Campaign.Client.Actors.Views;
using Motk.Shared.Descriptors;
using UnityEngine;

namespace Motk.Campaign.Client.Actors.Descriptors
{
  [Serializable]
  public class CharacterDescriptor : DescriptorBase
  {
    [field: SerializeField]
    public CampaignActorView Prefab { get; private set; } = null!;
  }
}