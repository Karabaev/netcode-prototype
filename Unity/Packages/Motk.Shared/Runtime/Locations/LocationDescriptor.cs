using System;
using Motk.Shared.Descriptors;
using UnityEngine;

namespace Motk.Shared.Locations
{
  [Serializable]
  public class LocationDescriptor : DescriptorBase
  {
    [field: SerializeField]
    public GameObject Prefab { get; private set; } = null!;
  }
}