using System;
using com.karabaev.descriptors.abstractions;
using UnityEngine;

namespace Motk.Combat.Client.Render.Units.Descriptors
{
  [Serializable]
  public class UnitVisualDescriptor : IDescriptor
  {
    [field: SerializeField]
    public CombatUnitView CombatPrefab { get; private set; } = null!;
  }
}