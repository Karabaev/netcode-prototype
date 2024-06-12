using System;
using Motk.Shared.Descriptors;
using UnityEngine;

namespace Motk.Shared.Characters
{
  [Serializable]
  public class CharacterDescriptor : DescriptorBase
  {
    [field: SerializeField]
    public GameObject Prefab { get; private set; } = null!;
  }
}