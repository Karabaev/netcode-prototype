using System;
using System.Collections.Generic;
using com.karabaev.descriptors.abstractions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Motk.Combat.Client.Render.Arena
{
  [Serializable]
  public class CombatArenaVisualDescriptor : IDescriptor
  {
    public AssetReferenceGameObject ArenaPrefab = null!;
    public IReadOnlyList<HeroPlacement> HeroPlacements = null!;
  }

  [Serializable]
  public class HeroPlacement
  {
    public Vector3 Position;
    public Vector3 Rotation;
  }
}