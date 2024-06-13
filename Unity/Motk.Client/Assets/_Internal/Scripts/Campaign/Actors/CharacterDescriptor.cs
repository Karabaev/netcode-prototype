using System;
using Motk.Shared.Descriptors;
using UnityEngine;

namespace Motk.Client.Campaign.Actors
{
  [Serializable]
  public class CharacterDescriptor : DescriptorBase
  {
    [field: SerializeField]
    public CampaignActorView Prefab { get; private set; } = null!;
  }
}