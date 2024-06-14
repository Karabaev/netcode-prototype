using System;
using Motk.Client.Campaign.Actors.Views;
using Motk.Shared.Descriptors;
using UnityEngine;

namespace Motk.Client.Campaign.Actors.Descriptors
{
  [Serializable]
  public class CharacterDescriptor : DescriptorBase
  {
    [field: SerializeField]
    public CampaignActorView Prefab { get; private set; } = null!;
  }
}