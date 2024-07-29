using System;
using com.karabaev.descriptors.abstractions;
using Motk.Client.Combat.Render;
using UnityEngine;

namespace Motk.Client.Units
{
  [Serializable]
  public class UnitVisualDescriptor : IDescriptor
  {
    [field: SerializeField]
    public CombatUnitVisualView CombatPrefab { get; private set; } = null!;
  }
}