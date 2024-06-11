using System;
using UnityEngine;

namespace Motk.Shared.Locations
{
  [Serializable]
  public class LocationDescriptor
  {
    [field: SerializeField]
    public string Id { get; private set; } = null!;

    [field: SerializeField]
    public GameObject Prefab { get; private set; } = null!;
  }
}