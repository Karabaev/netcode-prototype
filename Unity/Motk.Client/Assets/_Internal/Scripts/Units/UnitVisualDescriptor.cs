using System;
using com.karabaev.descriptors.abstractions;
using Motk.Client.Combat.Units;
using UnityEngine;

namespace Motk.Client.Units
{
  [Serializable]
  public class UnitVisualDescriptor : IDescriptor
  {
    [field: SerializeField]
    public CombatUnitView CombatPrefab { get; private set; } = null!;
  }
}